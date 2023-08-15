<template>
  <div>
    <Row class="form-row mb-2">
      <Col span="24">
        <span class="menu-btn">
          <div class="menu-btn">
            <Button
              class="btn btn--create ml-2"
              v-if="isAllowEdit"
              @click="handleEditNew"
              >{{ $t("common.edit") }}</Button
            >
            <Button
              class="btn btn--create ml-2"
              v-if="isAllowEdit"
              @click="deleteBlog"
              >{{ $t("policy.delete") }}</Button
            >
            <Button class="btn btn--back ml-2" @click="onbackToList">{{
              $t("common.backToList")
            }}</Button>
          </div>
        </span>
      </Col>
    </Row>
    <Row>
      <Col :xs="24" :sm="24" :md="16" :lg="16">
        <h3 style="margin-bottom:10px">
          <img
            class="img-avatar"
            v-bind:src="blogDetail.urlAvatar"
            alt="img"
            height="60"
          />
          <strong @click="openBlog(blogDetail.userId)">{{
            blogDetail.fullName
          }}</strong>
        </h3>
        <!-- <Row>
             <Col span="4" class="col">
             <span v-if="blogDetail.reaction == null"><Icon @click="onLikeBlog(blogDetail.postId)" class="icono-thumbs-up" type="md-thumbs-up" /> {{blogDetail.totalReaction}} {{ $t("blog.reaction")}}</span>
             <span v-else class="thumbs-up-on"><Icon @click="onLikeBlog(blogDetail.postId)" class="icono-thumbs-up" type="md-thumbs-up" /> {{blogDetail.totalReaction}} {{ $t("blog.reaction")}}</span>
             </Col>
             <Col span="4" class="col"><span class="glyphicon glyphicon-eye-open"></span><span> {{blogDetail.totalView}} {{ $t("blog.view")}}</span></Col>
          </Row> -->
        <div class="title-detail">
          {{ this.blogDetail.tilte }}
        </div>
        <div class="type-name">
          <p>{{ this.blogDetail.timeCreated | moment }}</p>
        </div>
        <Row style="margin-bottom:20px;margin-left:-13px">
          <Col span="4" class="col">
            <span v-if="blogDetail.reaction == null"
              ><Icon
                @click="onLikeBlog(blogDetail.postId)"
                class="icono-thumbs-up"
                type="md-thumbs-up"
              />
              {{ blogDetail.totalReaction }} {{ $t("blog.reaction") }}</span
            >
            <span v-else class="thumbs-up-on"
              ><Icon
                @click="onLikeBlog(blogDetail.postId)"
                class="icono-thumbs-up"
                type="md-thumbs-up"
              />
              {{ blogDetail.totalReaction }} {{ $t("blog.reaction") }}</span
            >
          </Col>
          <Col span="4" class="col"
            ><span class="glyphicon glyphicon-eye-open"></span
            ><span> {{ blogDetail.totalView }} {{ $t("blog.view") }}</span></Col
          >
        </Row>
        <div class="description" v-html="blogDetail.detail"></div>
      </Col>
      <Col :xs="24" :sm="24" :md="8" :lg="8">
        <Card class="quick-links-card">
          <h3 class="title-left">Quick Links</h3>
          <Row>
            <Col :xs="24" :sm="24" :md="24" :lg="24" style="padding-top: 10px;">
              <ul
                class="quick-links"
                style="margin-left: 10px; font-size: 18px; list-style: none; display: flex; justify-content: space-between;"
              >
                <li>
                  <b
                    ><a
                      href="https://komu.nccsoft.vn/"
                      target="_blank"
                      style="color: #fff; font-size: 15px;"
                      ><img
                        class="icon-new"
                        src="../../../../images/Komu.png"
                      /> </a
                  ></b>
                  <span class="tooltip-social">Komu</span>
                </li>
                <li>
                  <b
                    ><a
                      href="http://timesheet.nccsoft.vn/"
                      target="_blank"
                      style="color: #fff; font-size: 15px;"
                      ><img
                        class="icon-new"
                        src="../../../../images/Timesheet.png"
                      /> </a
                  ></b>
                  <span class="tooltip-social">Timesheet</span>
                </li>
                <li>
                  <b
                    ><a
                      href="https://ops.nccsoft.vn/"
                      target="_blank"
                      style="color: #fff; font-size: 15px;"
                      ><img
                        class="icon-new"
                        src="../../../../images/devOps.png"
                      /> </a
                  ></b>
                  <span class="tooltip-social">DevOps</span>
                </li>
                <li>
                  <b
                    ><a
                      href="http://talent.nccsoft.vn/"
                      target="_blank"
                      style="color: #fff; font-size: 15px;"
                      ><img
                        class="icon-new"
                        src="../../../../images/Talent-management.png"
                      /> </a
                  ></b>
                  <span class="tooltip-social">Talent management</span>
                </li>
              </ul>
            </Col>
          </Row>
          <Row>
            <Col :xs="24" :sm="24" :md="24" :lg="24" style="padding-top: 10px;">
              <ul
                class="quick-links"
                style="margin-left: 10px; font-size: 18px; list-style: none; display: flex; justify-content: space-between;"
              >
                <li>
                  <b
                    ><a
                      href="http://crm.nccsoft.vn/"
                      target="_blank"
                      style="color: #fff; font-size: 15px;"
                      ><img
                        class="icon-new"
                        src="../../../../images/CRM.png"
                      /> </a
                  ></b>
                  <span class="tooltip-social">CRM</span>
                </li>
                <li>
                  <b
                    ><a
                      href="http://hrm.nccsoft.vn/"
                      target="_blank"
                      style="color: #fff; font-size: 15px;"
                      ><img
                        class="icon-new"
                        src="../../../../images/HRM.png"
                      /> </a
                  ></b>
                  <span class="tooltip-social">HRM</span>
                </li>
                <li>
                  <b
                    ><a
                      href="http://finance.nccsoft.vn/"
                      target="_blank"
                      style="color: #fff; font-size: 15px;"
                      ><img
                        class="icon-new"
                        src="../../../../images/Finance.png"
                      /> </a
                  ></b>
                  <span class="tooltip-social">Finance</span>
                </li>
                <li>
                  <b
                    ><a
                      href="http://checkpoint.nccsoft.vn/"
                      target="_blank"
                      style="color: #fff; font-size: 15px;"
                      ><img
                        class="icon-new"
                        src="../../../../images/Checkpoint.png"
                      /> </a
                  ></b>
                  <span class="tooltip-social">Checkpoint</span>
                </li>
              </ul>
            </Col>
          </Row>
        </Card>

        <!--vùng lịch-->
        <Card class="calendar-card">
          <h3 class="title-left title-left-dd">
            {{ $t("dashboard.calendar") }}
          </h3>
          <div class="user-calendar">
            <Calendar :isReadOnly="true" :height="450" />
          </div>
        </Card>
      </Col>
    </Row>
    <!------vùng bình luận --------->
    <hr style="border-top: 1px solid #ccc;" />
    <Row class="row mb-2">
      <Col :xs="24" :sm="24" :md="16" :lg="16" class="col mt-4">
        <div style="width: 95%; float: right">
          <!---vùng nhập bình luận--->
          <div>
            <h4>Bình luận</h4>
            <Mentionable
              class=" comment"
              :keys="['@']"
              :items="items"
              :limit="5"
              :mapInsert="mapInsert"
              offset="6"
              ><textarea
                placeholder="Nhập bình luận......."
                class="text-area"
                rows="3"
                v-model="comment"
            /></Mentionable>
            <Button
              style="float:right"
              v-if="comment"
              @click="saveMainComment"
              size="small"
              class="btn-save-main-comment mt-2"
              type="primary"
            >
              <h4>{{ $t("common.send") }}</h4>
            </Button>
          </div>
          <!---vùng chọn xem tin mới nhất, nổi bật nhất--->
          <div style="margin-top: 30px">
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
            <hr style="border-bottom: 1px solid gray;" width="100%" />
          </div>
          <!-------Vùng hiển thị bình luận của tin mới nhất, nổi bật nhất----------->
          <div v-for="item in dataComment" :key="item.id">
            <div style="margin-top: 20px">
              <Row>
                <Col span="1">
                  <div>
                    <img
                      :src="item.avatar"
                      style="width: 52px;
                  border-radius:50%;
                  -moz-border-radius:50%;
                  -webkit-border-radius:50%;"
                    /></div></Col
                ><Col span="22" style="margin-left:5px">
                  <div>
                    <span
                      style="font-size: 15px; margin-right:5px; color: #04416D"
                      ><b>{{ item.userName }}</b></span
                    >
                    <span
                      v-if="!item.isShow"
                      style="color:#222; font-size:15px"
                      >{{ item.comment }}</span
                    >
                    <textarea
                      v-if="item.isShow"
                      placeholder="Nhập bình luận"
                      class="text-area"
                      rows="2"
                      v-model="item.editComment"
                    />
                    <div v-if="item.isShow" style="float:right">
                      <Button type="primary" @click="saveComment">{{
                        $t("common.save")
                      }}</Button>
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
                      <span class="action-icon" v-else
                        >{{ item.like
                        }}<Icon
                          @click="likeMainComment(item.id)"
                          class="icono-thumbs-up"
                          type="md-thumbs-up"
                      /></span>
                      <span
                        class="action-icon"
                        v-if="!isRely"
                        @click="showInputRely(item, 'Reply')"
                        >Trả lời</span
                      >
                      <span
                        class="action-icon"
                        v-else
                        @click="showInputRely(item, 'Noreply')"
                        >Trả lời</span
                      >
                      <!-- <span size="small" @click="getSubComment(item.id)" type="text"><span v-if="item.isSubComment"><p>{{$t('common.cleanups')}}</p></span><span v-else><p>{{$t('common.more')}}</p></span></span> -->
                      <span style="margin-right: 20px;" class="before-time">
                        <span v-if="item.month"
                          >{{ item.month }} {{ $t("calendar.month") }}</span
                        >
                        <span v-if="item.date"
                          >{{ item.date }} {{ $t("calendar.day") }}</span
                        >
                        <span v-if="!item.date && !item.month && !item.minutes"
                          >{{ item.hours }} {{ $t("common.hours") }}</span
                        >
                        <span v-if="!item.date && !item.month && !item.hours"
                          >{{ item.minutes }} {{ $t("common.minutes") }}</span
                        >

                        {{ $t("common.before") }}</span
                      >
                      <span
                        v-if="item.isRespone"
                        style="margin-right: 20px; cursor: pointer;"
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
                          <div slot="title" v-if="item.isEdit">
                            <a @click="openEditComment(item)">{{
                              $t("common.edit")
                            }}</a>
                          </div>
                          <div slot="content">
                            <a @click="deleteComment(item)">{{
                              $t("common.delete")
                            }}</a>
                          </div>
                        </Poptip>
                      </span>
                    </p>
                    <Mentionable
                      class=" comment"
                      :keys="['@']"
                      :items="items"
                      :limit="5"
                      :mapInsert="mapInsert"
                      offset="6"
                    >
                      <textarea
                        v-if="isRely && item.replyOfItem"
                        placeholder="Nhập trả lời......."
                        class="text-area"
                        rows="2"
                        v-model="item.subComment"
                      />
                    </Mentionable>
                    <Button
                      style="float:right"
                      v-if="item.subComment && isRely"
                      @click="saveSubComment(item.id)"
                      size="small"
                      class="btn-save-main-comment mt-2"
                      type="primary"
                    >
                      <h4>{{ $t("common.send") }}</h4>
                    </Button>
                    <p
                      style="margin-top:10px; font-size: 13px; color: #076DB6; cursor : pointer"
                      v-if="!item.isRespone && item.totalReply != 0"
                      @click="showSubFeedback(item, item.id, 'Reply')"
                    >
                      <span class="glyphicon glyphicon-share-alt" /><b>
                        {{ item.totalReply }} Trả lời</b
                      >
                    </p>
                  </div></Col
                ></Row
              >
            </div>
            <!----vùng hiển thị trả lời---->
            <div
              style="margin-top: 20px; margin-left:30px; border-left: 1px solid indianred;"
              v-if="item.isRespone"
            >
              <Row
                style="margin-left:15px"
                v-for="element in item.replyComment"
                :key="element.id"
              >
                <Col span="1">
                  <div>
                    <img
                      :src="url + element.avatar"
                      style="width: 52px;
                  border-radius:50%;
                  -moz-border-radius:50%;
                  -webkit-border-radius:50%;"
                    /></div
                ></Col>
                <Col span="23">
                  <div>
                    <span
                      style="font-size: 15px; margin-right:5px; color: #04416D"
                      @click="openBlog(element.userId)"
                      ><b>{{ element.userName }}</b></span
                    >
                    <span
                      v-if="!element.isShow"
                      style="color:#222; font-size:15px"
                      >{{ element.comment }}</span
                    >
                    <textarea
                      v-if="element.isShow"
                      placeholder="Nhập bình luận"
                      class="text-area"
                      rows="2"
                      v-model="element.editComment"
                    />
                    <div v-if="element.isShow" style="float:right">
                      <Button type="primary" @click="saveComment">{{
                        $t("common.save")
                      }}</Button>
                      <Button type="default" @click="cancel(element)">{{
                        $t("common.cancel")
                      }}</Button>
                    </div>

                    <p class="action-comment">
                      <!-- <span style="margin-right: 20px;">23 <Icon class="icono-thumbs-up" type="md-thumbs-up" /></span> -->
                      <span class="action-icon" v-if="!element.liked"
                        >{{ element.like }}
                        <Icon
                          @click="likeSubComment(element.id)"
                          class="icono-thumbs-up"
                          type="md-thumbs-up"
                      /></span>
                      <span class="action-icon" v-else
                        >{{ element.like }}
                        <Icon
                          @click="likeSubComment(element.id)"
                          class="icono-thumbs-up"
                          type="md-thumbs-up"
                      /></span>
                      <span
                        class="action-icon"
                        v-if="!isRelyChild"
                        @click="showInputChildRely(element, 'Reply')"
                        >Trả lời</span
                      >
                      <span
                        class="action-icon"
                        v-else
                        @click="showInputChildRely(element, 'NoReply')"
                        >Trả lời</span
                      >
                      <span class="before-time"
                        ><span v-if="element.month"
                          >{{ element.month }} {{ $t("calendar.month") }}</span
                        ><span v-if="element.date"
                          >{{ element.date }} {{ $t("calendar.day") }}</span
                        >
                        <span v-if="!element.date && !element.month"
                          >{{ element.hours }} {{ $t("common.hours") }}
                        </span>
                        {{ $t("common.before") }}</span
                      >
                      <span>
                        <Poptip
                          v-if="element.isEdit || isAdmin"
                          v-model="element.visible"
                          class="poptip-menu"
                        >
                          <i
                            data-v-7f89a61e=""
                            class="ivu-icon ivu-icon-md-more"
                          ></i>
                          <div slot="title" v-if="element.isEdit">
                            <a @click="openEditComment(element, 'sub')">{{
                              $t("common.edit")
                            }}</a>
                          </div>
                          <div slot="content">
                            <a @click="deleteComment(element, 'sub')">{{
                              $t("common.delete")
                            }}</a>
                          </div>
                        </Poptip>
                      </span>
                    </p>
                    <Mentionable
                      class=" comment"
                      :keys="['@']"
                      :items="items"
                      :limit="5"
                      :mapInsert="mapInsert"
                      offset="6"
                      ><textarea
                        v-if="isRelyChild && element.isReplySub"
                        placeholder="Nhập trả lời......."
                        class="text-area"
                        rows="2"
                        v-model="item.subComment"
                      />
                    </Mentionable>
                    <Button
                      style="float:right"
                      v-if="item.subComment && isRelyChild"
                      @click="saveSubComment(item.id)"
                      size="small"
                      class="btn-save-main-comment mt-2"
                      type="primary"
                    >
                      <h4>{{ $t("common.send") }}</h4>
                    </Button>
                  </div>
                </Col>
              </Row>
            </div>
          </div>
        </div>
      </Col>
      <Col
        v-if="isSeeMore"
        span="16"
        style="text-align: center;margin-top:20px"
      >
        <span @click="seeMore()">
          <strong style="color: #076DB6;font-size: 15px; cursor: pointer;"
            >Tải thêm bình luận....
          </strong></span
        >
      </Col>
    </Row>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Prop, Watch } from "vue-property-decorator";
import { EStatus } from "../../../../types/information/EStatus";
import { EPermission } from "../../../../types/information/EPermission";
import SessionStore from "../../../../store/modules/session";
import url from "../../../../lib/url";
import BannerCarousel from "../../../../components/banner-carousel.vue";
import EditComment from "../../../../components/edit-comment.vue";
import moment from "moment";
import BlogModule from "@/store/modules/blog";
import { Mentionable } from "vue-mention";
import Calendar from "../../../../components/fullCalendar/calendar.vue";
@Component({
  components: { BannerCarousel, EditComment, Mentionable, Calendar },
  props: {},
  filters: {
    moment(value: any) {
      return moment(value).format("HH:mm YYYY-MM-DD");
    },
  },
})
export default class DetailListBlog extends Vue {
  @Prop() newsSelected: {};
  private order: number = 1;
  private pageSizeComment: number = 5;
  private isSeeMore: boolean = false;
  private blogDetail: any = {};
  private EStatus: any = EStatus;
  private url = url;
  private SessionStore = SessionStore;
  private isViewMyblog: boolean = false;
  private isNew: boolean = false;
  private comment: string = "";
  private dataComment: any = [];
  private isSubComment: boolean = false;
  private dataEditComment: any = {};
  private dataDefaultComment: any = {};
  private entityName: string = "Blog";
  private entityId: string = "";
  private userId: number = null;
  private isAdmin: boolean = false;
  private isAllowEdit = false;
  private items: any = [];
  private userIds: any = [];
  private isFeedback = false;
  private isRely = false;
  private isRelyChild = false;
  @Watch("$route")
  getNews() {
    this.fetchData();
  }
  showInputRely(item, string) {
    if (string === "Reply") {
      item.replyOfItem = true;
      this.isRely = !this.isRely;
    } else {
      item.replyOfItem = false;
      this.isRely = !this.isRely;
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
  async created() {
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

  async fetchData() {
    this.isUser();
    this.getComment();
    if (this.$route.params.blogId)
      this.blogDetail = await this.$store.dispatch({
        type: "blog/detailAPost",
        id: this.$route.params.blogId,
      });
    this.isAllowEdit =
      this.SessionStore.state.user.id === this.blogDetail.userId;
    this.blogDetail.urlAvatar = url + this.blogDetail.urlAvatar;
    this.blogDetail.detail = this.getHasshtag(this.blogDetail.detail);
  }

  getHasshtag(data) {
    let detail = "";
    let before = data.slice(0, data.indexOf("#")); // chuỗi trc hashtag
    let after = data.slice(data.indexOf("#"), data.length); // chuỗi còn lại
    let final = after.slice(after.indexOf("#"), after.indexOf(" ")); // chuỗi hashtag
    let afterfinal = after.slice(final.length, after.length); // chuỗi sau hashtag
    if (afterfinal.includes("#")) {
      afterfinal = this.getHasshtag(afterfinal);
    }
    let hashtag = final.link("#");
    detail = before + hashtag.bold() + afterfinal;
    return detail;
  }
  beforeMount() {
    this.fetchData();
  }

  onbackToList() {
    if (this.$route.name === "MyblogView") {
      this.$router.push(`/blog/myblog`);
    } else {
      this.$router.push(`/blog/listblog`);
    }
  }
  handleEditNew() {
    this.$router.push({
      name: "EditMyblog",
      params: { myblogId: this.$route.params.blogId },
    });
  }

  private async onLikeBlog(id) {
    let formData = {
      postId: id,
      reaction: 1,
    };
    let response = await this.$store.dispatch({
      type: "blog/reactionPost",
      data: formData,
    });
    this.blogDetail.reaction = response.reaction;
    this.blogDetail.totalReaction = response.totalReact;
  }

  private async deleteBlog() {
    await this.$store.dispatch({
      type: "blog/deleteBlogPost",
      idDelete: this.$route.params.blogId,
    });
    this.$router.push("/blog/myblog");
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
          element.like = element.like - 1;
        }
      }
    });
  }

  private async likeSubComment(id: number) {
    await this.$store.dispatch({
      type: "news/likeSubComment",
      idSubComment: id,
    });
    this.getComment();
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
      entityId: this.$route.params.blogId,
      entityName: "Blog",
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
    this.dataComment = mainComment.items.map((el) => {
      let newEl = el;
      newEl.newComment = el.comment
        ? this.getUserInComment(el.comment).el_comment
        : "";
      newEl.editComment = el.comment
        ? this.getUserInComment(el.comment).editComment
        : "";
      newEl.avatar = url + el.avatar;
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
      newEl.avatar = el.avatar;
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
    const newComment = this.fixComment(this.comment);
    let params = {
      entityId: this.$route.params.blogId,
      entityName: "Blog",
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
  }

  private async saveSubComment(id: number) {
    let subComment = "";
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
  }
  private openEditComment(item: any, isSub?: string) {
    if (!isSub) {
      this.isSubComment = false;
    } else {
      this.isSubComment = true;
    }
    this.dataEditComment = item;
    this.dataDefaultComment = { ...item };
    item.isShow = !item.isShow;
    this.entityId = this.$route.params.blogId;
    this.$forceUpdate();
  }
  private async deleteComment(item: any, isSub?: string) {
    if (!isSub) {
      await this.$store.dispatch({
        type: "news/deleteMainComment",
        idComment: item.id,
      });
      this.getComment();
    } else {
      await this.$store.dispatch({
        type: "news/deleteSubComment",
        idSubComment: item.id,
      });
      this.getComment();
    }
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

  async saveComment() {
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

  cancel(item: any, isSub?: string) {
    item.isShow = !item.isShow;
    item.comment = this.dataDefaultComment.comment;
    this.$forceUpdate();
  }

  openBlog(id: any) {
    this.$router.push({ name: "userBlog", params: { userId: id } });
  }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
.title-detail {
  font-size: 21px;
  font-weight: bold;
  color: black;
  span {
    color: red;
  }
}
.title-left {
  position: relative;
  padding-bottom: 6px;
}
.icon-new {
  width: 50px;
}
.img-avatar {
  width: 100px;
  height: 100px;
  border-radius: 50%;
  margin-right: 10px;
}
.quick-links li {
  margin-bottom: 10px;
  border-radius: 2px;
  position: relative;
}
.quick-links li .tooltip-social {
  visibility: hidden;
  background-color: #fff;
  color: #000;
  text-align: center;
  padding: 3px 5px;
  max-width: 125px;
  border: 1px solid black;
  font-size: 13px;
  white-space: nowrap;
  /* Position the tooltip */
  position: absolute;
  left: -22px;
  z-index: 1;
}
.quick-links li:hover .tooltip-social {
  visibility: visible;
}
.quick-links li a {
  display: block;
  width: 100%;
}
.quick-links-card {
  margin-bottom: 20px;
  border: none;
}
.quick-links-card:hover {
  box-shadow: none;
}
.calendar-card {
  border: none;
}
.calendar-card:hover {
  box-shadow: none;
}
.user-calendar {
  border: 1px solid #dcdee2;
  padding: 10px;
}
.title-left {
  text-align: left;
  font-size: 18px;
  margin-bottom: 10px;
  font-weight: bold;
  text-transform: uppercase;
  color: indianred;
  position: relative;
  padding-bottom: 6px;
}
.title-left::before {
  height: 1px;
  display: block;
  margin-top: 3px;
  width: 100%;
  background: #ccc;
  content: "";
  position: absolute;
  left: 0;
  bottom: 0;
}
.lastestNew {
  font-size: 18px;
  margin-top: 50px;
  margin-right: 15px;
  cursor: pointer;
}
.active1 {
  color: #9f224e !important;
  font-weight: 700;
}
.active2 {
  color: #9f224e !important;
  font-weight: 700;
  cursor: pointer;
}
.action-comment {
  margin-top: 10px;
  margin-bottom: 20px;
  font-size: 13px;
  .action-icon {
    margin-right: 20px;
    cursor: pointer;
  }
}
.type-name {
  justify-content: space-between;
  font-size: 12px;
  margin-bottom: 10px;
  //padding-bottom: 5px;
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
.list-comment {
  min-height: 100%;
  padding: 20px 0px 0px 0px;
  .btn-comment:first-child {
    margin-right: 20px;
  }
  .name-user {
    color: #2d8cf0;
    margin-left: 10px;
  }
  .comment {
    padding-top: 20px;

    .reply-comment {
      margin-left: 25px;
      padding: 10px;
      .write-feedback {
        margin-top: 20px;
        position: relative;
        .btn-save-main-comment {
          margin-top: 10px;
          height: 30px;
        }
      }
    }
    pre {
      font-family: "Open Sans", Arial, sans-serif;
      white-space: pre-wrap;
      word-break: break-word;
    }
    .action {
      padding: 0 15px;
      display: flex;
      justify-content: space-between;
      .icono-thumbs-up {
        font-size: 20px;
      }
      .before-time {
        min-width: 60px;
      }
      span {
        display: flex;
        // padding-top: 15px;
        p {
          padding-top: 5px;
          padding-right: 3px;
        }
      }
    }
  }
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
}
.comment-zone {
  margin-top: 7px;
  padding: 1px 15px;
  background-color: #fff;
  border-radius: 12px;
}
.text-area {
  width: 100%;
  padding: 7px;
}
</style>
<style>
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
</style>
