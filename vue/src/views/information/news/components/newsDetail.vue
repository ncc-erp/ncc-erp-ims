<template>
  <div class="dashboard">
    <Row class="form-row group-btn">
      <Col span="24">
        <span class="menu-btn">
          <div class="menu-btn">
            <span
              v-if="
                this.news.status == EStatus.WaitingForApproval &&
                SessionStore.state.user.canWaitingFromDraft
              "
              class="col-label"
              >{{ $t("common.waitForApprove") }}</span
            >
            <span
              v-if="
                this.news.status == EStatus.Approved &&
                SessionStore.state.user.canWaitingFromDraft
              "
              class="col-label mr-2"
              >{{ $t("common.approved") }}</span
            >
            <Button
              v-if="
                this.news.status == EStatus.Draft &&
                SessionStore.state.user.canWaitingFromDraft
              "
              @click="changeStatus(EStatus.WaitingForApproval)"
              class="btn btn--create mr-2"
              type="info"
              >{{ $t("common.submit") }}</Button
            >
            <Button
              v-if="
                this.news.status == EStatus.Return &&
                SessionStore.state.user.canWaitingFromReturn
              "
              @click="changeStatus(EStatus.WaitingForApproval)"
              class="btn btn--create mr-2"
              type="info"
              >{{ $t("common.submitAgain") }}</Button
            >
            <Button
              v-if="
                this.news.status == EStatus.WaitingForApproval &&
                SessionStore.state.user.canAppoveFromWaiting
              "
              @click="changeStatus(EStatus.Approved)"
              class="btn btn--create mr-2"
              type="info"
              >{{ $t("common.approve") }}</Button
            >
            <Button
              v-if="
                this.news.status == EStatus.Return &&
                SessionStore.state.user.canAppoveFromReturn
              "
              @click="changeStatus(EStatus.Approved)"
              class="btn btn--create mr-2"
              type="info"
              >{{ $t("common.approve") }}</Button
            >
            <Button
              v-if="
                this.news.status == EStatus.WaitingForApproval &&
                SessionStore.state.user.canReturnFromWaiting
              "
              @click="changeStatus(EStatus.Return)"
              class="btn btn--create mr-2"
              type="info"
              >{{ $t("common.return") }}</Button
            >
            <Button
              v-if="
                this.news.status == EStatus.Approved &&
                SessionStore.state.user.canHiddenFromApprove
              "
              @click="changeStatus(EStatus.Hidden)"
              class="btn btn--create mr-2"
              type="info"
              >{{ $t("common.hidden") }}</Button
            >
            <Button
              v-if="
                this.news.status == EStatus.Approved &&
                SessionStore.state.user.canReturnFromApprove
              "
              @click="changeStatus(EStatus.Disabled)"
              class="btn btn--create mr-2"
              type="info"
              >{{ $t("common.disable") }}</Button
            >
            <Button
              class="btn btn--create mr-2"
              v-if="
                this.news.canEdit || SessionStore.state.user.role == 'Admin'
              "
              @click="handleEditNew"
              >{{ $t("common.edit") }}</Button
            >
            <!-- <Button class="btn btn--back mr-2" @click="backToList">{{ $t("common.backToList")}}</Button> -->
          </div>
        </span>
      </Col>
    </Row>
    <Row
      class="wrapper-post"
      style="
        
        background: white;
        display: flex;
        justify-content: space-between;
      "
    >
      <!-- <Col
        class="col col__item col-offset quick-links-container left-db"
        :xs="24"
        :sm="5"
        :md="5"
        :lg="5"
      >
        <Row>
          <LayoutLeftPost></LayoutLeftPost>
        </Row>
      </Col> -->

      <Col
        :xs="24"
        :sm="24"
        :md="14"
        :lg="14"
        class="col col__item wrap-content information-detail"
      >
        <!-- <div class="content-news">
        <div class="title-detail">
          {{this.news.title}}
        </div>
        <div class="type-name">
          <span class="news-type">{{this.news.typeName}}</span>
          <p style="color: #777; font-size: 14px; margin-right: 15px;">{{(this.news.modifiedDate ? this.news.modifiedDate : this.news.createDate) | moment}}</p>
        </div>
        
        <img :src="this.news.image" alt="img" class="img-news" />
        <div class="description" v-html="news.description" style="margin-top:15px"></div></div> -->

        <div class="content-news">
          <div class="title-detail">
            {{ this.news.title }}
          </div>
          <div class="type-name">
            <span class="news-type">{{ this.news.typeName }}</span>
            <p style="color: #777; font-size: 14px; margin-right: 15px">
              {{
                (this.news.modifiedDate
                  ? this.news.modifiedDate
                  : this.news.createDate) | moment
              }}
            </p>
          </div>
          <div class="box-img">
            <img :src="this.news.image" alt="img" class="img-news" />
          </div>
          <div class="description" v-html="news.description"></div>
          <div class="wrapper-favourite">
            <div class="d-flex favourite">
              <div>{{ news.userLike.totalLike }} Thích</div>
              <div>{{ news.totalComment }} bình luận</div>
            </div>
            <hr />
            <div
              class="d-flex"
              style="justify-content: space-around; height: 42px"
            >
              <div class="action-favourite like-btn" @click="actionLike()">
                <img
                  height="22px"
                  v-if="!news.userLike.isLiked"
                  src="../../../../images/Like.svg"
                />
                <img height="22px" v-else src="../../../../images/Liked.svg" />
                <div v-bind:class="{ liked: isLiked }">Thích</div>
              </div>
              <div class="action-favourite" @click="showComment()">
                <img height="22px" src="../../../../images/Comment.svg" />
                Bình luận
              </div>
            </div>
            <hr />
          </div>
        </div>
        <Col
          :xs="24"
          :sm="24"
          :md="24"
          :lg="24"
          class="box-comment-wrap"
          v-if="this.isShowComment"
        >
          <div class="comment-wrap" style="width: 100%">
            <!---vùng chọn xem tin mới nhất, nổi bật nhất--->
            <div class="wrapper-comment">
              <span
                class="lastestNew"
                @click="getComment('new')"
                v-bind:class="{ active1: isNew }"
                >{{ $t("common.new") }}</span
              >
              <span
                class="lastestNew"
                @click="getComment('prominent')"
                v-bind:class="{ active2: !isNew }"
                >{{ $t("common.highlights") }}</span
              >
            </div>
            <!-------Vùng hiển thị bình luận của tin mới nhất, nổi bật nhất----------->
            <div v-for="item in dataComment" :key="item.id">
              <div style="padding-bottom: 10px">
                <Row class="d-flex">
                  <Col span="1">
                    <div style="width: 50px; height: 50px">
                      <img
                        :src="item.avatar"
                        style="
                          width: 46px;
                          height: 46px;
                          border-radius: 50%;
                          -moz-border-radius: 50%;
                          -webkit-border-radius: 50%;
                        "
                      /></div></Col
                  ><Col span="23" class="box-content">
                    <div>
                      <span
                        style="
                          font-size: 15px;
                          margin-right: 5px;
                          color: #04416d;
                          vertical-align: top;
                        "
                        ><b>{{ item.userName }}</b></span
                      >
                      <pre v-if="!item.isShow" class="show-comment-box">{{
                        item.comment
                      }}</pre>

                      <textarea
                        v-if="item.isShow"
                        placeholder="Viết bình luận"
                        class="text-area edit-box"
                        rows="2"
                        @keydown.enter.exact.prevent="saveComment"
                        @keydown.enter.shift.exact.prevent="
                          item.editComment += '\n'
                        "
                        @keydown.enter.ctrl.exact.prevent="
                          item.editComment += '\n'
                        "
                        v-model="item.editComment"
                      />
                      <div
                        v-if="item.isShow"
                        style="
                          float: right;
                          margin-top: 10px;
                          margin-bottom: 10px;
                        "
                      >
                        <Button
                          type="primary"
                          class="btn-save"
                          @click="saveComment"
                          >{{ $t("common.save") }}</Button
                        >
                        <Button type="default" @click="cancel(item)">{{
                          $t("common.cancel")
                        }}</Button>
                      </div>

                      <p class="action-comment">
                        <span class="action-icon" v-if="!item.liked"
                          >{{ item.like }}
                          <Icon
                            @click="likeMainComment(item.id)"
                            class="icono-thumbs-up"
                            type="md-thumbs-up"
                        /></span>
                        <span class="action-icon" style="color: #0068ca" v-else
                          >{{ item.like
                          }}<Icon
                            @click="likeMainComment(item.id)"
                            class="icono-thumbs-up"
                            style="color: #0068ca"
                            type="md-thumbs-up"
                        /></span>
                        <button
                          class="action-icon reply-btn"
                          v-if="!isRely"
                          :disabled="isActive && item.replyOfItem"
                          v-bind:class="{
                            noReply: isActive && item.replyOfItem,
                          }"
                          @click="showInputRely(item, 'Reply')"
                        >
                          Trả lời
                        </button>
                        <button
                          class="action-icon reply-btn"
                          v-else
                          :disabled="isActive && !item.replyOfItem"
                          v-bind:class="{
                            noReply: isActive && !item.replyOfItem,
                          }"
                          @click="showInputRely(item, 'Noreply')"
                        >
                          Trả lời
                        </button>
                        <!-- <span size="small" @click="getSubComment(item.id)" type="text"><span v-if="item.isSubComment"><p>{{$t('common.cleanups')}}</p></span><span v-else><p>{{$t('common.more')}}</p></span></span> -->
                        <span class="before-time">
                          <span v-if="item.month"
                            >{{ item.month }} {{ $t("calendar.month") }}</span
                          >
                          <span v-if="item.date"
                            >{{ item.date }} {{ $t("calendar.day") }}</span
                          >
                          <span
                            v-if="!item.date && !item.month && !item.minutes"
                            >{{ item.hours }} {{ $t("common.hours") }}</span
                          >
                          <span v-if="!item.date && !item.month && !item.hours"
                            >{{ item.minutes }} {{ $t("common.minutes") }}</span
                          >

                          {{ $t("common.before") }}
                        </span>
                        <span
                          v-if="item.isRespone"
                          class="ml-2 cursor-p un-expand"
                          @click="showSubFeedback(item, item.id, 'Collapse')"
                          >Thu gọn</span
                        >
                        <span
                          v-if="item.isRespone"
                          class="collapse"
                          @click="showSubFeedback(item, item.id, 'Collapse')"
                          >Thu gọn</span
                        >
                        <span>
                          <Poptip
                            v-if="item.isEdit || isAdmin"
                            v-model="item.visible"
                            class="poptip-menu"
                          >
                            <i
                              data-v-7f89a61e=""
                              class="ivu-icon ivu-icon-md-more"
                            ></i>
                            <div
                              @click="openEditComment(item)"
                              class="submit-btn"
                              slot="title"
                              v-if="item.isEdit"
                            >
                              <a>{{ $t("common.edit") }}</a>
                            </div>
                            <div
                              slot="content"
                              class="delete-btn"
                              @click="deleteComment(item)"
                            >
                              <a>{{ $t("common.delete") }}</a>
                            </div>
                          </Poptip>
                        </span>
                      </p>
                      <Mentionable
                        :keys="['@']"
                        :items="items"
                        :limit="5"
                        :mapInsert="mapInsert"
                        offset="6"
                      >
                        <textarea
                          v-if="isRely && item.replyOfItem && isMain"
                          placeholder="Nhập trả lời......."
                          class="text-area reply-box"
                          rows="2"
                          @keydown.enter.exact.prevent="
                            saveSubComment(item.id, item.subComment)
                          "
                          @keydown.enter.shift.exact.prevent="
                            item.subComment += '\n'
                          "
                          @keydown.enter.ctrl.exact.prevent="
                            item.subComment += '\n'
                          "
                          v-model="item.subComment"
                        />
                      </Mentionable>
                      <Button
                        style="float: right"
                        v-if="item.subComment && isRely && isMain"
                        @click="cancelReplyMainComment(item)"
                        size="medium"
                        class="btn-save-main-comment mt-2"
                        type="default"
                      >
                        <h4 class="cancel-button">
                          {{ $t("common.cancel") }}
                        </h4>
                      </Button>
                      <Button
                        style="float: right"
                        v-if="item.subComment && isRely && isMain"
                        @click="saveSubComment(item.id, item.subComment)"
                        size="medium"
                        class="btn-save-main-comment mt-2 mr-2"
                        type="primary"
                      >
                        <h4 class="sending-button">
                          {{ $t("common.send") }}
                        </h4>
                      </Button>
                      <p
                        class="btn-show-reply"
                        v-if="!item.isRespone && item.totalReply != 0"
                        @click="showSubFeedback(item, item.id, 'Reply')"
                      >
                        <span class="glyphicon glyphicon-share-alt" />
                        {{ item.totalReply }} Trả lời
                      </p>
                    </div></Col
                  ></Row
                >
              </div>
              <!----vùng hiển thị trả lời---->
              <div class="box-child-reply" v-if="item.isRespone">
                <Row
                  style="margin-left: 15px"
                  v-for="element in item.replyComment"
                  :key="element.id"
                >
                  <Col span="2">
                    <div style="padding: 0px">
                      <img
                        :src="element.avatar"
                        style="
                          height: 45px;
                          border-radius: 50%;
                          width: 45px;
                          -moz-border-radius: 50%;
                          -webkit-border-radius: 50%;
                        "
                      /></div
                  ></Col>
                  <Col span="22" class="box-reply">
                    <div>
                      <span @click="openBlog(element.userId)"
                        ><b>{{ element.userName }}</b></span
                      >
                      <pre class="show-comment-box" v-if="!element.isShow">{{
                        element.comment
                      }}</pre>
                      <textarea
                        v-if="element.isShow"
                        placeholder="Viết bình luận"
                        class="text-area edit-box"
                        rows="2"
                        @keydown.enter.exact.prevent="saveComment"
                        @keydown.enter.shift.exact.prevent="
                          element.editComment += '\n'
                        "
                        @keydown.enter.ctrl.exact.prevent="
                          element.editComment += '\n'
                        "
                        v-model="element.editComment"
                      />
                      <div
                        v-if="element.isShow"
                        style="
                          float: right;
                          margin-top: 10px;
                          margin-bottom: 10px;
                        "
                      >
                        <Button
                          type="primary"
                          class="btn-save"
                          @click="saveComment"
                          >{{ $t("common.save") }}</Button
                        >
                        <Button type="default" @click="cancel(element)">{{
                          $t("common.cancel")
                        }}</Button>
                      </div>

                      <p class="action-comment">
                        <!-- <span style="margin-right: 20px;">23 <Icon class="icono-thumbs-up" type="md-thumbs-up" /></span> -->
                        <span class="action-icon" v-if="!element.liked"
                          >{{ element.like }}
                          <Icon
                            @click="likeSubComment(item.id, element.id)"
                            class="icono-thumbs-up"
                            type="md-thumbs-up"
                        /></span>
                        <span class="action-icon" style="color: #0068ca" v-else
                          >{{ element.like }}
                          <Icon
                            @click="likeSubComment(item.id, element.id)"
                            class="icono-thumbs-up"
                            style="color: #0068ca"
                            type="md-thumbs-up"
                        /></span>
                        <button
                          class="action-icon reply-btn"
                          :disabled="
                            isActive && !element.isReplySub && isRelyChild
                          "
                          v-bind:class="{
                            noReply:
                              isActive && !element.isReplySub && isRelyChild,
                          }"
                          v-if="!isRelyChild"
                          @click="showInputChildRely(element, item, 'Reply')"
                        >
                          Trả lời
                        </button>
                        <button
                          class="action-icon reply-btn"
                          :disabled="
                            isActive && !element.isReplySub && isRelyChild
                          "
                          v-bind:class="{
                            noReply:
                              isActive && !element.isReplySub && isRelyChild,
                          }"
                          v-else
                          @click="showInputChildRely(element, item, 'NoReply')"
                        >
                          Trả lời
                        </button>
                        <span class="before-time"
                          ><span v-if="element.month"
                            >{{ element.month }}
                            {{ $t("calendar.month") }}</span
                          ><span v-if="element.date"
                            >{{ element.date }} {{ $t("calendar.day") }}</span
                          >
                          <span v-if="!element.date && !element.month"
                            >{{ element.hours }} {{ $t("common.hours") }}
                          </span>
                          {{ $t("common.before") }}</span
                        >
                        <span class="more-child">
                          <Poptip
                            v-if="element.isEdit || isAdmin"
                            v-model="element.visible"
                            class="poptip-menu"
                          >
                            <i
                              data-v-7f89a61e=""
                              class="ivu-icon ivu-icon-md-more"
                            ></i>
                            <div
                              slot="title"
                              class="submit-btn"
                              @click="openEditComment(element, 'sub')"
                              v-if="element.isEdit"
                            >
                              <a>{{ $t("common.edit") }}</a>
                            </div>
                            <div
                              slot="content"
                              class="delete-btn"
                              @click="deleteComment(element, 'sub')"
                            >
                              <a>{{ $t("common.delete") }}</a>
                            </div>
                          </Poptip>
                        </span>
                      </p>
                      <Mentionable
                        class="comment"
                        :keys="['@']"
                        :items="items"
                        :limit="5"
                        :mapInsert="mapInsert"
                        offset="6"
                      >
                        <textarea
                          v-if="isRelyChild && element.isReplySub"
                          placeholder="Nhập trả lời......."
                          class="text-area reply-box"
                          @keydown.enter.exact.prevent="
                            saveSubComment(item.id, element.subComment)
                          "
                          @keydown.enter.shift.exact.prevent="
                            item.subComment += '\n'
                          "
                          @keydown.enter.ctrl.exact.prevent="
                            item.subComment += '\n'
                          "
                          role="textbox"
                          contenteditable
                          rows="2"
                          v-model="item.subComment"
                        />
                      </Mentionable>
                      <Button
                        style="float: right"
                        v-if="item.subComment && element.isReplySub"
                        @click="cancelReplySubComment(element, item)"
                        size="medium"
                        class="btn-save-main-comment mt-2"
                        type="default"
                      >
                        <h4 class="cancel-button">
                          {{ $t("common.cancel") }}
                        </h4>
                      </Button>
                      <Button
                        style="float: right"
                        v-if="item.subComment && element.isReplySub"
                        @click="saveSubComment(item.id, element.subComment)"
                        size="medium"
                        class="btn-save-main-comment mt-2 mr-2"
                        type="primary"
                      >
                        <h4 class="sending-button">{{ $t("common.send") }}</h4>
                      </Button>
                    </div>
                  </Col>
                </Row>
              </div>
            </div>
            <Row>
              <Col v-if="isSeeMore" span="16" class="box-comment">
                <span @click="seeMore()">
                  <strong
                    v-if="!loadMore"
                    style="color: #076db6; font-size: 15px; cursor: pointer"
                    >Tải thêm bình luận ....
                  </strong></span
                >
              </Col>
              <Col span="24">
                <scroll-loader v-show="loadMore"> </scroll-loader>
              </Col>
            </Row>
            <!---vùng nhập bình luận--->
            <div>
              <h4></h4>
              <Mentionable
                class="comment"
                :keys="['@']"
                :items="items"
                :limit="5"
                :mapInsert="mapInsert"
                offset="6"
              >
                <span class="avatar"><img :src="Avartar" /></span>
                <textarea
                  placeholder="Viết bình luận......."
                  class="text-area reply-main"
                  :class="{ send: comment, nsend: !comment }"
                  @keydown.enter.exact.prevent="saveMainComment"
                  @keydown.enter.shift.exact.prevent="comment += '\n'"
                  @keydown.enter.ctrl.exact.prevent="comment += '\n'"
                  rows="1"
                  v-model="comment"
                />
                <emoji-picker @emoji="insert" :search="search">
                  <div
                    class="emoji-invoker"
                    slot="emoji-invoker"
                    slot-scope="{ events: { click: clickEvent } }"
                    @click.stop="clickEvent"
                  >
                    <img
                      height="30"
                      style="margin-right: 20px; cursor: pointer"
                      src="../../../../images/smile.png"
                    />
                  </div>
                  <div slot="emoji-picker" slot-scope="{ emojis, insert }">
                    <div class="emoji-picker">
                      <div
                        class="text-center emoji-picker__search"
                        style="margin-top: 3px"
                      >
                        <input type="text" v-model="search" />
                      </div>
                      <div class="list-emoji">
                        <div
                          v-show="Object.keys(emojis).length > 0"
                          v-for="(emojiGroup, category) in emojis"
                          :key="category"
                        >
                          <h5 class="mt-2">{{ category }}</h5>
                          <div class="emojis">
                            <span
                              class="cursor-p"
                              v-for="(emoji, emojiName) in emojiGroup"
                              :key="emojiName"
                              @click="insert(emoji)"
                              :title="emojiName"
                              >{{ emoji }}</span
                            >
                          </div>
                        </div>
                        <div
                          v-show="Object.keys(emojis).length == 0"
                          class="emoji-picker__no-results"
                        >
                          No results for "{{ search }}".
                        </div>
                      </div>
                    </div>
                  </div>
                </emoji-picker>
                <Button
                  style="margin-right: 5px"
                  v-if="comment"
                  @click="saveMainComment"
                  size="small"
                  class="btn-save-main-comment send-comment"
                  type="primary"
                >
                  <!-- <h4 class="sending-button">{{ $t("common.send") }}</h4> -->
                  <!-- <i class="fa fa-paper-plane"></i> -->
                  <img width="34" src="../../../../images/send.png" />
                </Button>
              </Mentionable>
            </div>
            <!----------------------------->
          </div>
        </Col>
      </Col>
      <Col
        :xs="24"
        :sm="24"
        :md="8"
        :lg="8"
       
        class="right-db"
      >
        <h3 class="relation">Tin liên quan</h3>

        <Row
          v-for="(news, index) in allNews"
          :key="index"
          align-v="center"
          class="d-flex justify-content-center"
        >
          <Col
            class="col col__mb col__item m-content box-news"
            :xs="24"
            :sm="24"
            :md="24"
            :lg="24"
          >
            <NewsItem :data="news" @click.native="openEventDetail(news.id)" />
          </Col>
        </Row>
        <div class="text-center">
          <h4
            style="
              margin-top: 27px;
              margin-bottom: 20px;
              cursor: pointer;
              font-size: 16px;
            "
            @click="seeMoreNews"
          >
            <span
              style="
                background: indianred;
                font-size: 14px;
                padding: 5px 17px;
                color: #fff;
              "
              >Xem thêm</span
            >
          </h4>
        </div>
      </Col>
    </Row>
  </div>
</template>
<script lang="ts">
import { Component, Prop, Vue, Watch, Emit } from "vue-property-decorator";
import { EStatus } from "../../../../types/information/EStatus";
import url from "../../../../lib/url";
import { EPermission } from "../../../../types/information/EPermission";
import SessionStore from "../../../../store/modules/session";
import BannerCarousel from "../../../../components/banner-carousel.vue";
import ScrollLoader from "vue-scroll-loader";
import moment from "moment";
import EditComment from "../../../../components/edit-comment.vue";
import { Mentionable } from "vue-mention";
import NewsItem from "../../../../components/news-item.vue";
import LayoutLeftPost from "../../../../components/layout-left-post.vue";
Vue.use(ScrollLoader);
@Component({
  components: {
    BannerCarousel,
    EditComment,
    Mentionable,
    NewsItem,
    LayoutLeftPost,
  },
  props: {},
  filters: {
    moment(value: any) {
      return moment(value).format("HH:mm YYYY-MM-DD");
    },
  },
})
export default class NewsDetail extends Vue {
  @Prop() newsSelected: {};
  private order: number = 1;
  private pageSizeComment: number = 5;
  private isSeeMore: boolean = false;
  private news: any = {};
  private search: string = "";
  private isShowComment: boolean = true;
  private amount: number;
  private loadMore: boolean;
  private isActive: boolean = false;
  private isChildActive: boolean = false;
  private isLiked = false;
  private isMain = true;
  private listFace = [];
  private EStatus = EStatus;
  private SessionStore = SessionStore;
  private isNew: boolean = false;
  private comment: string = "";
  private dataComment: any = [];
  private isSubComment: boolean = false;
  private dataEditComment: any = {};
  private dataDefaultComment: string;
  private entityName: string = "New";
  private entityId: string = "";
  private userId: number = null;
  private isAdmin: boolean = false;
  private items: any = [];
  private userIds: any = [];
  private isFeedback = false;
  private isRely = false;
  private isRelyChild = false;
  // @Watch("$route")
  getNews() {
    this.fetchData();
  }
  async created() {
    this.fetchData();
    let response = await this.$store.dispatch({
      type: "news/getAllUser",
    });
    this.items = response.map((el) => {
      let item: any = {};
      item.value = el.fullName;
      item.label = el.fullName;
      item.id = el.userId;
      return item;
    });
    
    
  }

  private mapInsert(item) {
    return "(" + item.label + ")";
  }
  insert(emoji) {
    this.comment += emoji;
  }
  private async actionLike() {
    if (this.news.userLike.isLiked) {
      this.news.userLike.isLiked = false;
    } else {
      this.news.userLike.isLiked = true;
    }
    let data = await this.$store.dispatch({
      type: "news/sendLikePost",
      id: this.$route.params.newsId,
    });
    this.getTotalLike();
  }

  private async getTotalLike() {
    let data = await this.$store
      .dispatch({
        type: "news/getTotalLike",
        id: this.$route.params.newsId,
      })
      .catch(() => {});
    this.news.userLike.isLiked = data.isLiked;
    this.news.userLike.totalLike = data.totalLike;
  }

  private getUserInComment(data: string) {
    let comment = data;
    let el_comment: any = [];
    let commentData: any = {
      editComment: "",
      el_comment: [],
    };
    while (comment.indexOf("-)") >= 0) {
      el_comment.push(comment.slice(0, comment.indexOf("-)") + 2));
      comment = comment.slice(comment.indexOf("-)") + 2);
    }
    el_comment.push(comment);
    el_comment = el_comment.map((el) => {
      let _el: any = {};
      if (el.indexOf("@(") >= 0 && el.indexOf(")&id=(-") >= 0) {
        _el.content = el.slice(0, el.indexOf("@("));
        _el.user = "@" + el.slice(el.indexOf("@(") + 2, el.indexOf(")&id=(-"));
        _el.userId = Number(
          el.slice(el.indexOf(")&id=(-") + 7, comment.indexOf("-)") - 1)
        );
        commentData.editComment +=
          el.slice(0, el.indexOf("@(")) +
          "@" +
          el.slice(el.indexOf("@(") + 1, el.indexOf(")&id=(-") + 1);
      } else {
        _el.content = el;
        _el.user = "";
        _el.userId = null;
        commentData.editComment += el;
      }
      return _el;
    });
    commentData.el_comment = el_comment;
    return commentData;
  }
  private fixComment(data: string) {
    let comment = data;
    let el_comment: any = [];
    let newComment = "";
    this.userIds = [];
    while (comment.indexOf(")") >= 0) {
      el_comment.push(comment.slice(0, comment.indexOf(")") + 1));
      comment = comment.slice(comment.indexOf(")") + 1);
      // do something
    }
    el_comment.push(comment);
    el_comment.map((el) => {
      let isUser: boolean = false;
      if (el.indexOf("@(") >= 0 && el.indexOf(")") >= 0) {
        this.items.forEach((element) => {
          if (
            el.slice(el.indexOf("@(") + 2, comment.indexOf(")")) ===
            element.label
          ) {
            isUser = true;
            newComment += el + "&id=(-" + element.id + "-)";
            this.userIds.push(element.id);
          }
        });
        if (!isUser) {
          newComment += el;
        }
      } else {
        newComment += el;
      }
    });
    return newComment;
  }

  private allNews = [];
  async getAllNews() {
    let response = await this.$store.dispatch({
      type: "news/getAllNews",
      data: {
        pageNumber: 1,
        pageSize: 5,
      },
    });
    this.allNews = response.items;
    this.allNews.forEach((el) => {
      el.isImage = el.image == null ? false : true;
      el.image = url + el.image;
      el.coverImage = url + el.coverImage;
      el.entityName = el.typeName;
    });
  }

  openEventDetail(id: number) {
    this.$router.push(`/information/news/${id}`);
    this.fetchData();
    this.comment = null;
  }

  async fetchData() {
    this.isUser();
    this.getAllNews();
    this.getComment();
    // this.getTotalLike();
    if (this.$route.params.newsId) {
      this.news = await this.$store.dispatch({
        type: "news/getNews",
        id: this.$route.params.newsId,
      });
      this.news.image = url + this.news.image;
    }
    // this.viewFace();
  }
  // async viewFace() {
  //   let response;
  //   response = await this.$store.dispatch({
  //     type: "dashboard/getListImage",
  //   });
  //   this.listFace = response.items.map((el) => {
  //     el.isApprove = el.clockStatus === "APPROVED" ? true : false;
  //     return el;
  //   });
  // }

  async showComment() {
    this.isShowComment = !this.isShowComment;
  }

  handleEditNew() {
    this.$router.push({
      name: "EditNews",
      params: { newsId: this.$route.params.newsId },
    });
  }

  beforeMount() {
    this.fetchData();
  }

  backToList() {
    this.$router.push("/information/news");
  }
  seeMoreNews() {
    this.$router.push("/information/news");
  }
  showInputRely(item, string) {
    this.isActive = true;
    if (string === "Reply") {
      item.replyOfItem = true;
      this.isRely = !this.isRely;
    } else {
      item.replyOfItem = false;
      this.isRely = !this.isRely;
    }
  }
  showInputChildRely(element, item, string) {
    this.isActive = true;
    this.isMain = !this.isMain;
    if (string === "Reply") {
      item.replyOfItem = true;
      element.isReplySub = true;
      this.isRely = !this.isRely;
      this.isRelyChild = !this.isRelyChild;
    } else {
      item.replyOfItem = false;
      element.isReplySub = false;
      this.isRely = !this.isRely;
      this.isRelyChild = !this.isRelyChild;
    }
  }
  showSubFeedback(item, id, string) {
    this.isFeedback = !this.isFeedback;
    if (string == "Collapse") {
      item.isRespone = false;
      return;
    }
    item.isRespone = true;
  }

  async changeStatus(status) {
    let newsData = this.news;
    newsData.status = status;
    newsData.coverImage = "";
    newsData.image = "";
    let formData = new FormData();
    for (let key in newsData) {
      if (key != "groupId") formData.append(key, newsData[key]);
      else {
        newsData.groupId.map((group) => {
          formData.append("groupId", group);
        });
      }
    }
    await this.$store.dispatch({
      type: "news/createEditNew",
      dataNews: formData,
    });
    this.fetchData();
  }

  editEvents() {
    this.$router.push({
      name: "EditNews",
      params: { newsId: this.$route.params.newsId },
    });
  }

  private async likeMainComment(id: number) {
    await this.$store.dispatch({
      type: "news/likeMainComment",
      idComment: id,
    });
    this.dataComment.forEach((element) => {
      if (element.id === id) {
        element.liked = !element.liked;
        if (element.liked) {
          element.like++;
        } else {
          element.like--;
        }
      }
    });
  }

  private async likeSubComment(idMainComment: number, idSubComment: number) {
    await this.$store.dispatch({
      type: "news/likeSubComment",
      idSubComment: idSubComment,
    });
    this.dataComment.forEach((item) => {
      if (item.id === idMainComment) {
        item.replyComment.forEach((element) => {
          if (element.id === idSubComment) {
            element.liked = !element.liked;
            if (element.liked) {
              element.like++;
            } else {
              element.like--;
            }
          }
        });
      }
    });
  }

  private seeMore() {
    this.pageSizeComment = this.pageSizeComment + 5;
    this.getComment();
  }

  async getComment(status?: String) {
    this.dataComment = [];
    if (status) {
      this.isNew = !this.isNew;
      if (status === "new") {
        this.order = 1;
      } else this.order = 2;
    }
    let params = {
      entityId: this.$route.params.newsId,
      entityName: "New",
      pageNumber: 1,
      pageSize: this.pageSizeComment,
      order: this.order,
    };

    const mainComment = await this.$store.dispatch({
      type: "news/getMainCommentPaging",
      mainComment: params,
    });

    if (mainComment.total > this.pageSizeComment) {
      this.isSeeMore = true;
    } else {
      this.isSeeMore = false;
    }
    this.loadMore = true;
    setTimeout(() => {
      this.dataComment = mainComment.items.map((el) => {
        let newEl = el;
        newEl.newComment = el.comment
          ? this.getUserInComment(el.comment).el_comment
          : "";
        newEl.editComment = el.comment
          ? this.getUserInComment(el.comment).editComment
          : "";
        if (el.avatar) {
          newEl.avatar = el.avatar.includes("http")
            ? el.avatar
            : url + el.avatar;
        }
        (newEl.subComment = ""), (newEl.visible = false);
        newEl.isRespone = false;
        newEl.replyOfItem = false;
        if (this.userId === el.userId) {
          newEl.isEdit = true;
        } else {
          newEl.isEdit = false;
        }
        let currentDate = new Date();
        let commentDate = new Date(el.time);
        var months;
        months = (currentDate.getFullYear() - commentDate.getFullYear()) * 12;
        months -= commentDate.getMonth();
        months += currentDate.getMonth();
        newEl.month = months <= 0 ? 0 : months;

        if (!newEl.month) {
          newEl.date = new Date().getDate() - new Date(el.time).getDate();
        }
        if (!newEl.date) {
          newEl.hours = new Date().getHours() - new Date(el.time).getHours();
        }
        if (!newEl.hours) {
          let minutes = currentDate.getTime() - commentDate.getTime();
          newEl.minutes = (minutes / 60000).toFixed();
        }
        (newEl.isSubComment = false), (newEl.replyComment = []);
        return newEl;
      });
      this.dataComment.forEach((element) => {
        this.getSubComment(element.id);
      });
      this.loadMore = false;
    }, 0);
  }

  private async getSubComment(id?: number) {
    let params = {
      mainCommentId: id,
      pageNumber: 1,
      pageSize: 100,
    };
    let dataSubComment = await this.$store.dispatch({
      type: "news/getSubCommentPaging",
      subComment: params,
    });

    const newSubComment = dataSubComment.map((el: any) => {
      let newEl = el;
      newEl.newComment = el.comment
        ? this.getUserInComment(el.comment).el_comment
        : "";
      newEl.editComment = el.comment
        ? this.getUserInComment(el.comment).editComment
        : "";
      newEl.avatar = el.avatar.includes("http") ? el.avatar : url + el.avatar;
      newEl.visible = false;
      newEl.mainId = id;
      if (this.userId === el.userId) {
        newEl.isEdit = true;
      } else {
        newEl.isEdit = false;
      }
      newEl.month = new Date().getMonth() - new Date(el.time).getMonth();
      if (!newEl.month) {
        newEl.date = new Date().getDate() - new Date(el.time).getDate();
      }
      if (!newEl.date) {
        newEl.hours = new Date().getHours() - new Date(el.time).getHours();
      }
      return newEl;
    });
    this.dataComment = this.dataComment.map((el) => {
      if (el.id === id) {
        el.replyComment = newSubComment;
        el.isSubComment = !el.isSubComment;
      }
      return el;
    });
  }

  private async saveMainComment() {
    if (this.comment) {
      this.loadMore = true;
      this.isActive = false;
      const newComment = this.fixComment(this.comment);
      let params = {
        entityId: this.$route.params.newsId,
        entityName: "New",
        comment: newComment,
        userIds: this.userIds,
      };
      let formData = new FormData();
      for (let key in params) {
        if (key != "userIds") {
          formData.append(key, params[key]);
        } else {
          this.userIds.forEach((element) => {
            formData.append(key, element);
          });
        }
      }
      await this.$store.dispatch({
        type: "news/saveMainComment",
        dataComment: formData,
      });
      this.getComment();
      this.comment = "";
      this.news.totalComment++;
    }
  }

  private async saveSubComment(id: number, childComment?: string) {
    if (childComment) {
      let subComment = "";
      this.loadMore = true;
      this.isActive = false;
      this.isMain = !this.isMain;
      this.isRelyChild = !this.isRelyChild;
      this.dataComment.forEach((element) => {
        if (element.id === id) {
          subComment = this.fixComment(element.subComment);
        }
      });
      let params = {
        mainCommentId: id,
        comment: subComment,
        userIds: this.userIds,
      };
      let formData = new FormData();
      for (let key in params) {
        if (key != "userIds") {
          formData.append(key, params[key]);
        } else {
          this.userIds.forEach((element) => {
            formData.append(key, element);
          });
        }
      }
      await this.$store.dispatch({
        type: "news/saveSubComment",
        dataSubComment: formData,
      });
      this.getComment();
      this.news.totalComment++;
    }
  }

  private openEditComment(item: any, isSub?: string) {
    if (!isSub) {
      this.isSubComment = false;
    } else {
      this.isSubComment = true;
    }
    item.visible = !item.visible;
    this.dataDefaultComment = item.comment;
    this.dataEditComment = item;
    item.isShow = !item.isShow;
    this.entityId = this.$route.params.newsId;
    this.$forceUpdate();
  }
  private async activeDeleteComment(item: any, isSub?: string) {
    if (!isSub) {
      await this.$store.dispatch({
        type: "news/deleteMainComment",
        idComment: item.id,
      });
      let amount = item.totalReply + 1;
      while (amount) {
        this.news.totalComment--;
        amount--;
      }
      this.getComment();
    } else {
      await this.$store.dispatch({
        type: "news/deleteSubComment",
        idSubComment: item.id,
      });
      this.news.totalComment--;
      this.getComment();
    }
  }
  private async deleteComment(item: any, isSub?: string) {
    this.$fire({
      title: "Are you sure?",
      text: "Delete this comment?",
      showCancelButton: true,
      confirmButtonColor: "#d33",
      cancelButtonColor: "#3085d6",
      confirmButtonText: "Yes, delete it!",
    }).then((r) => {
      if (r.value) {
        this.activeDeleteComment(item, isSub);
        item.visible = !item.visible;
      }
    });
  }
  async isUser() {
    const response = await this.$store.dispatch({
      type: "news/init",
    });
    this.userId = response.user.id;
    if (response.user.role === "Admin") {
      this.isAdmin = true;
    }
  }

  url: string = url;
  get Avartar() {
    let avartar;
    if (this.$store.state.session.user.avatar) {
      if (this.$store.state.session.user.avatar.includes("http")) {
        avartar = this.$store.state.session.user.avatar;
      } else {
        avartar = url + this.$store.state.session.user.avatar;
      }
    } else {
      avartar = "../images/avatar.svg";
    }
    return avartar;
  }

  async saveComment() {
    if (this.dataEditComment.editComment) {
      if (this.isSubComment) {
        let params = {
          mainCommentId: this.dataEditComment.mainId,
          comment: this.fixComment(this.dataEditComment.editComment),
          id: this.dataEditComment.id,
        };
        let formData = new FormData();
        for (let key in params) {
          formData.append(key, params[key]);
        }
        await this.$store.dispatch({
          type: "news/saveSubComment",
          dataSubComment: formData,
        });
        this.getComment();
      } else {
        let params = {
          entityId: this.entityId,
          entityName: this.entityName,
          comment: this.fixComment(this.dataEditComment.editComment),
          id: this.dataEditComment.id,
        };
        let formData = new FormData();
        for (let key in params) {
          formData.append(key, params[key]);
        }
        await this.$store.dispatch({
          type: "news/saveMainComment",
          dataComment: formData,
        });
        this.getComment();
      }
    }
  }

  private cancel(item: any) {
    item.isShow = !item.isShow;
    item.editComment = this.dataDefaultComment;
    this.$forceUpdate();
  }
  private cancelReplyMainComment(item: any) {
    this.isRely = !this.isRely;
    this.isActive = false;
    item.subComment = "";
    item.replyOfItem = false;
  }
  private cancelReplySubComment(item: any, mainComment: any) {
    this.isMain = !this.isMain;
    mainComment.subComment = "";
    item.subComment = "";
    this.isActive = false;
    item.isReplySub = false;
    this.isRelyChild = !this.isRelyChild;
  }
  private openBlog(id: any) {
    this.$router.push({ name: "userBlog", params: { userId: id } });
  }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
* {
  color: #333;
}
.send-comment {
  padding: 0 !important;
  border: none;
  background: #ffffff;
}
.group-btn {
  background: white;
}
.like-btn {
  align-items: normal !important;
}
.list-item {
  .tag {
    display: none;
  }
}
.reply-box {
  border: 1px solid #f1f1f1 !important;
  border-radius: 20px;
  margin-top: 10px;
  width: 100%;
}
.active1 {
  color: #9f224e !important;
  font-weight: 700;
  cursor: pointer;
  padding-bottom: 5px;
  border-bottom: 1px solid #b24747;
}
.avatar {
  margin-left: 15px;
  height: 43px;
  width: 50px;
  margin-right: 10px;
}
.active2 {
  color: #9f224e !important;
  font-weight: 700;
  cursor: pointer;
  padding-bottom: 5px;
  border-bottom: 1px solid #b24747;
}
.list-item {
  margin: 0 auto;
}
.box-comment-wrap {
  display: flex;
  padding: 0px 10px 5px;
}
.edit-box {
  border: 1px solid #f1f1f1 !important;
  border-radius: 20px;
  width: 100%;
}
.content-news {
  width: 100%;
  float: right;
  padding: 10px 10px 20px;
}
::v-deep .ivu-poptip {
  cursor: pointer;
}
::v-deep .ivu-card-body {
  margin-left: -0.5px;
}
.show-comment-box {
  padding: 0;
  margin: 0;
  word-break: break-word;
  color: #000000;
  font-size: 15px;
  white-space: pre-line;
  background: #ffff;
  border: none;
  font-family: sans-serif;
}
.information-detail {
  // margin-left: 24.5%;
  // padding: 0 40px;
  margin-bottom: 15px;
  margin-top: 0;
}
.btn-save {
  color: #ffffff;
  margin-right: 5px;
}
.reply-btn {
  font-family: "Segoe UI", "sans-serif";
  color: #757575;
  font-size: 16px;
  border: none;
  background: transparent;
  height: 20px;
  :hover {
    background: #ffff;
  }
}
.mr-2 {
  margin-right: 5px;
}
.glyphicon {
  top: 3px;
  color: #757575;
}
.mr-2 {
  margin-right: 5px;
}
.glyphicon {
  top: 3px;
  color: #757575;
}
.left-db {
  padding-left: 0px;
  position: fixed;
  height: 91vh;
  overflow: scroll;
  width: 19.25vw;
  padding-right: 0px;
  ::-webkit-scrollbar {
    display: none;
  }
}
.user-calendar {
  border: none;
}
.box-news {
  margin-bottom: 18px;
}
::v-deep .box-news .ivu-card-body {
  height: 290px;
}
.news-item {
  padding-left: 0;
}
.img-news {
  width: 70%;
}
.box-img {
  display: flex;
}
.submit-btn:hover {
  cursor: pointer;
  a {
    color: #2b85e4;
  }
}
.delete-btn:hover {
  cursor: pointer;
  a {
    color: #ec2323;
  }
}
.time-create {
  color: #b7b7b7;
  font-size: 16px;
  margin-right: 15px;
  font-weight: 100;
  padding-top: 10px;
}
.description {
  margin-top: 20px;
  margin-bottom: 10px;
  font-size: 18px;
  word-break: break-word;
}
.box-comment {
  text-align: left;
  margin-bottom: 10px;
  padding-left: 40px;
}
.box-reply {
  margin-left: -15px;
}
.box-child-reply {
  font-size: 15px;
  margin-right: 5px;
  margin-bottom: 10px;
  margin-left: 30px;
  border-left: 1px solid indianred;
}
.btn-show-reply {
  font-size: 13px;
  cursor: pointer;
  color: #757575;
  padding-left: 10px;
}
.box-comment {
  text-align: left;
  margin-bottom: 10px;
  padding-left: 40px;
}
.box-reply {
  margin-left: -15px;
}
.box-child-reply {
  font-size: 15px;
  margin-right: 5px;
  margin-bottom: 10px;
  margin-left: 30px;
  border-left: 1px solid indianred;
}
.btn-show-reply {
  font-size: 13px;
  cursor: pointer;
  color: #757575;
  padding-left: 10px;
}
.img-face {
  max-width: 300px;
  height: 90px;
  width: 90px;
  object-fit: cover;
}
.image-face {
  width: 90px;
  display: flex;
  justify-content: center;
}
.option {
  padding-left: 16px;
}
.item {
  margin-bottom: 15px;
}
.banner {
  margin-bottom: 30px;
}
.more {
  font-size: 16px;
  cursor: pointer;
  font-weight: 700;
  width: fit-content;
}
.icon-new {
  height: 60px;
  padding-bottom: 6px;
}
.title-left,
.related_news {
  text-align: left;
  font-size: 23px;
  margin-bottom: 10px;
  font-weight: bold;
  text-transform: uppercase;
  color: #000000;
}
.accepted {
  font-size: 15px;
  color: #858585;
  display: flex;
  align-items: center;
  img {
    filter: grayscale(1);
    padding-right: 5px;
    padding-bottom: 4px;
  }
}
.title-left {
  position: relative;
  padding-bottom: 12px;
  font-family: Segoe UI;
}
.liked {
  color: #0068ca;
  font-weight: 700;
}
::v-deep .footer-left {
  display: none;
}
.content-adjustment {
  display: flex;
  justify-self: center;
  margin-bottom: 20px;
}
.collapse {
  margin-right: 20px;
  cursor: pointer;
}
.wrapper-comment {
  margin-bottom: 20px;
}
.title-left-treasury {
  position: relative;
  padding-bottom: 8px;
  font-family: Segoe UI;
  text-align: left;
  font-size: 23px;
  font-weight: bold;
  text-transform: uppercase;
  color: #000000;
}
.reply-main {
  border: none;
}
.noReply {
  cursor: not-allowed !important;
}
.wrapper-favourite {
  margin: 0 auto;
}
.quick-links-card::before {
  height: 1px;
  display: block;
  margin-top: 3px;
  width: 100%;
  background: #ccc;
  content: "";
  position: absolute;
  left: 0;
  bottom: 5px;
}
.action-favourite {
  display: flex;
  align-items: flex-start;
  width: 20%;
  border-bottom: 1px solid #b24747;
  justify-content: center;
  align-items: center;
  font-size: 18px;
  padding: 10px;
  font-weight: bold;
  cursor: pointer;
  img {
    padding-right: 10px;
  }
}
.my-face-card {
  box-shadow: none;
  border: none;
}
.wrapper-post {
  overflow: hidden;
}
.un-expand {
  margin-left: 0;
}
.favourite {
  width: 85%;
  justify-content: space-between;
  margin: 0 auto;
  padding: 10px;
  font-size: 18px;
}
.format {
  padding-left: 13px;
  padding-top: 10px;
  background-color: #f3f3f3;
}
.related_news::after {
  height: 1px;
  display: block;
  margin-top: 3px;
  width: 97%;
  background: #ccc;
  content: "";
}
.title-right {
  text-align: right;
}
.box-content {
  margin-left: 32px;
  padding-right: 3vw;
}
p {
  text-transform: uppercase;
  font-size: 18px;
  font-weight: bold;
  padding: 5px 0;
}
.lastest-news {
  margin-bottom: 45px;
}
.user-calendar {
  padding: 10px;
}

.btn-option {
  border: none;
  background-color: #f3f3f3;
  padding: 5px 0px 6px;
  width: 65px;
}
.accept {
  font-size: 15px;
  color: green;
  display: flex;
  align-items: center;
  img {
    padding-right: 5px;
  }
}
.reject {
  font-size: 15px;
  margin-top: 2px;
  color: red;
  display: flex;
  align-items: center;
  img {
    padding-right: 5px;
  }
}

.title-detail {
  word-break: break-word;
  font-size: 24px;
  line-height: 24px;
  font-weight: bold;
  color: black;
  span {
    color: red;
  }
}
.type-name {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  font-size: 17px;
  margin-bottom: 20px;
  padding-bottom: 5px;
}
.news-type {
  background: #ec2323;
  color: #fff;
  padding: 1px 13px;
  font-size: 14px;
  margin-right: 10px;
}
::v-deep .col-label {
  padding-top: 0.65rem;
  line-height: 0.5;
}
.btn-zone {
  float: right;
}
.banner-carousel {
  margin-bottom: 20px;
}
.action-comment {
  font-size: 13px;
  display: flex;
  align-items: end;
  .action-icon {
    text-transform: none;
    margin-right: 10px;
    cursor: pointer;
    display: flex;
    align-items: center;
  }
  span:last-child {
    margin-top: -1.35px;
  }
  .more-child {
    margin-top: -1.5px;
  }
}
.relation {
  position: relative;
  padding-bottom: 6px;
  text-align: left;
  font-size: 18px;
  margin-bottom: 14px;
  font-weight: bold;
  text-transform: uppercase;
  color: indianred;
  margin-left: 15px;
}
.relation::after {
  height: 1px;
  display: block;
  margin-top: 3px;
  width: 97%;
  background: #ccc;
  content: "";
}
.information {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  width: 100%;
  img {
    width: 32px;
    border-radius: 50%;
    -moz-border-radius: 50%;
    -webkit-border-radius: 50%;
  }

  .poptip-menu {
    position: absolute;
    right: 20px;
    font-size: 32px;
  }
}
.thumbs-up-on {
  color: blue;
}
.btt-see-more {
  margin-top: 30px;
}
.menu-btn {
  float: right;
  margin-right: 15px;
}
.comment-zone {
  margin-top: 7px;
  padding: 1px 15px;
  border-radius: 12px;
}
/* ::v-deep .box {
  height: 8.96vw !important;
} */
.send {
  width: 95%;
}
.nsend {
  width: 100%;
}
.text-area {
  padding: 5px 13px;
  overflow: hidden;
  font-style: normal;
  font-variant-ligatures: normal;
  font-variant-caps: normal;
  font-variant-numeric: normal;
  font-variant-east-asian: normal;
  font-weight: 400;
  font-stretch: normal;
  font-size: 15px;
  font-family: arial;
  resize: vertical;
  display: block;
  outline: none;
}
.lastestNew {
  font-size: 16px;
  margin-top: 50px;
  margin-right: 15px;
  cursor: pointer;
}
::v-deep {
  .ivu-card-body {
    padding: 0px;
  }
}
.comment:nth-child(2) {
  display: flex;
  align-items: center;
  border-top: 1px solid #f1f1f1;
  border-bottom: 1px solid #f1f1f1;
  position: relative;
  padding: 10px 0 10px 0;
  margin: 0 auto 20px;
}

.width_common {
  width: 100%;
  float: left;
}
.before-time {
  margin-right: 10px;
}
.sending-button {
  color: #fff;
  font-size: 14px;
  font-weight: 700;
}
.cancel-button {
  color: #000000;
  font-size: 14px;
  font-weight: 700;
}
.action-comment span,
.action-comment span i {
  color: rgb(117, 117, 117);
}
.action-comment span:nth-child(1) i {
  margin-left: 5px;
}
.mention-item {
  padding: 4px 10px;
  border-radius: 4px;
}

.mention-selected {
  background: rgb(192, 250, 153);
}
.vue-popover-theme {
  background: #fff;
  z-index: 10000;
}
.show-comment {
  padding: 10px 5px;
}
.popover {
  display: block;
}
.tooltip {
  opacity: 100;
}
.tooltip-inner {
  background: #fff;
  color: #000;
}
strong:hover {
  color: #2d8cf0;
  border-bottom: 1px solid #2d8cf0;
}
.name-user:hover {
  border-bottom: 1px solid #2d8cf0;
}
.emoji-invoker {
  position: relative;
  cursor: pointer;
  transition: all 0.2s;
}

.emoji-invoker:hover {
  transform: scale(1.1);
}

.emoji-invoker > svg {
  fill: #b1c6d0;
}

.emoji-picker {
  position: absolute;
  bottom: 50px;
  right: 15px;
  z-index: 1;
  font-family: "Montserrat", sans-serif;
  color: #606f7b;
  border: 1px solid #ccc;
  overflow: auto;
  padding: 1rem;
  border-radius: 0.25rem;
  background: #fff;
  box-shadow: 1px 1px 8px #c7dbe6;
  line-height: 1.5;
}

.emoji-picker__search {
  width: 100%;
}

.emoji-picker__search > input {
  border-radius: 0.25rem;
  border: 1px solid #ccc;
  padding: 0.5rem;
  outline: none;
  width: 100%;
}

.emoji-picker h5 {
  margin-bottom: 0.25rem;
  text-transform: uppercase;
  letter-spacing: 0.05rem;
  font-size: 0.7rem;
  cursor: default;
}

.emoji-picker__no-results {
  margin-top: 1rem;
  font-size: 0.75rem;
  word-wrap: break-word;
}

.emoji-picker .emojis {
  display: grid;
  grid-template-columns: repeat(6, 1fr);
  grid-gap: 0.25rem;
}

.emoji-picker .emojis:after {
  content: "";
}

.emoji-picker .emojis span {
  padding: 0.2rem;
  cursor: pointer;
  border-radius: 0.25rem;
  display: flex;
  align-items: center;
  justify-content: center;
}

.emoji-picker .emojis span:hover {
  background: #ececec;
  cursor: pointer;
}

.boder {
  margin-top: 100px;
  border-width: 1px;
  border-bottom: 1px solid gray;
}
@media only screen and (max-width: 1440px) {
  .box-news {
    padding-left: 0px !important;
    padding-right: 0px !important;
  }
}
@media only screen and (max-width: 1050px) {
  ::v-deep .box-news .ivu-card-body {
    height: 220px;
    border-radius: 5px;
  }
  .box-reply {
    margin-left: 0px;
    float: right;
    width: 88%;
  }
  .action-favourite {
    padding: 10px 5px;
    width: 30%;
  }
  img {
    max-width: none;
  }
  .action-favourite {
    width: 30%;
  }
  .box-content {
    margin-left: 40px;
    padding-right: 3vw;
  }
  .box-reply {
    margin-left: 0px;
    width: 87%;
    float: right;
  }
  .avatar {
    width: 55px;
    margin-right: 0;
  }
  .send {
    width: 87%;
  }
  .action-comment {
    .action-icon {
      margin-right: 10px;
    }
    .collapse {
      margin-right: 10px;
    }
    .before-time {
      margin-right: 5px;
    }
    .poptip-menu {
      margin-bottom: 2px;
    }
  }
  img {
    max-width: none;
  }
}
.dashboard{
  padding-top: 35px;
  background: white;
}
/* @media only screen and (max-width: 968px) {
  .content-news {
    width: 100%;
    float: right;
  }
  .box-content {
    padding-right: 6vw;
  }
} */
@media only screen and (max-width: 970px) {
  .left-db {
    width: 34% !important;
  }
  ::v-deep .box-news .ivu-card-body {
    height: 300px;
    border-radius: 5px;
  }
  .list-item {
    padding: 1.2vw;
    padding-left: 0;
  }
  .box-news {
    margin-left: 0px;
    margin-bottom: 0px;
  }
  .information-detail {
    margin-left: 37%;
    width: 62% !important;
    padding: 0 20px;
  }
  .right-db {
    width: 62% !important;
  }
  .action-favourite {
    width: 40%;
  }
  .right-db {
    float: right;
  }
}
@media only screen and (max-width: 517px) {
  .information-detail {
    margin-left: 0;
    width: 100% !important;
    padding: 0 20px;
  }
  .right-db {
    width: 100% !important;
  }
  .box-content {
    margin-left: 35px;
    padding-right: 3vw;
  }
  .show-comment-box {
    padding-right: 10vw;
  }
  .box-news {
    padding-left: 10px !important;
    padding-right: 10px !important;
  }
  .list-item {
    padding-right: 0;
  }
  .left-db {
    height: auto;
    width: 100% !important;
    position: relative;
  }
  .reply-btn {
    font-size: 14px;
  }
  .box-reply {
    width: 84%;
  }
  .action-comment .action-icon {
    padding: 0px;
  }
  .comment-wrap {
    button {
      padding: 5px 10px 6px;
    }
  }
  .nsend {
    width: 86%;
  }
  .send {
    width: 74%;
  }
}
@media only screen and (max-width: 415px) {
  .box-news {
    margin-bottom: 10px;
  }
  .box-comment-wrap {
    padding: 0px;
  }
  .action-favourite {
    width: 42%;
    padding: 10px 0px;
  }
  .send {
    width: 70%;
  }
  .box-content {
    margin-left: 10.5vw;
    padding-right: 3vw;
  }
  .edit-box {
    width: 90%;
  }
  .box-reply {
    width: 83%;
  }
}
</style>
