<template>
  <Modal v-model="data.isShow" width="30%" z-index="0" class="modal-comment">
    <div slot="header" class="box-header" @click="showTitle()" align="center">
      <div class="d-flex j-left pb-5">
        <div class="info-header">
          <div class="badge badge-pill badge-danger bg-like alg-center">
            Like:
            <p class="badge badge-light total-cmt">
              {{ data.userLike.totalLike }}
            </p>
          </div>
          <div class="badge badge-pill badge-danger bg-cmt alg-center">
            Comment:
            <p class="badge badge-light total-cmt">
              {{ data.totalComment }}
            </p>
          </div>
          <div class="createDate bg-date badge badge-light alg-center">
            Create:
            <p class="badge badge-light total-cmt">
              {{ data.createDate | moment }}
            </p>
          </div>
        </div>
      </div>
      <label
        class="fs-18"
        :class="{ titleDialogHide: isShowTitle, titleDialogShow: !isShowTitle }"
        ><span
          style="word-break: break-word !important ; white-space: normal"
          >{{ data.title }}</span
        ></label
      >
    </div>
    <div class="body-list-comment" style="z-index: 1">
      <div
        v-for="item in dataComment"
        :key="item.id"
        id="listCommentId"
        ref="listCommentId"
      >
        <div class="mt-10">
          <Row>
            <Col span="1">
              <div class="box-image-avatar">
                <img class="image-avatar" :src="item.avatar" /></div></Col
            ><Col span="22">
              <div class="form-content-comment">
                <span class="text-username"
                  ><b>{{ item.userName }}</b></span
                >
                <br />
                <pre v-if="!item.isShow" class="text-content-comment">{{
                  item.comment
                }}</pre>
                <textarea
                  v-if="item.isShow"
                  placeholder="Viết bình luận"
                  class="text-area edit-box"
                  js-autoresize
                  rows="1"
                  @keydown.enter.exact.prevent="saveComment"
                  @keydown.enter.shift.exact.prevent="item.editComment += '\n'"
                  @keydown.enter.ctrl.exact.prevent="item.editComment += '\n'"
                  v-model="item.editComment"
                />
                <div v-if="item.isShow" class="f-right">
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
                    v-bind:class="{ noReply: isActive && item.replyOfItem }"
                    @click="showInputRely(item, 'Reply')"
                  >
                    Trả lời
                  </button>
                  <button
                    class="action-icon reply-btn"
                    v-else
                    :disabled="isActive && !item.replyOfItem"
                    v-bind:class="{ noReply: isActive && !item.replyOfItem }"
                    @click="showInputRely(item, 'Noreply')"
                  >
                    Trả lời
                  </button>
                  <!-- <span size="small" @click="getSubComment(item.id)" type="text"><span v-if="item.isSubComment"><p>{{$t('common.cleanups')}}</p></span><span v-else><p>{{$t('common.more')}}</p></span></span> -->
                  <span class="before-time mr-20">
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

                    {{ $t("common.before") }}
                  </span>
                  <span
                    v-if="item.isRespone"
                    class="ml-2 cursor-p un-expand"
                    @click="showSubFeedback(item, item.id, 'Collapse')"
                    >Thu gọn</span
                  >
                  <!-- <span
                    v-if="item.isRespone"
                    class="collapse"
                    @click="showSubFeedback(item, item.id, 'Collapse')"
                    >Thu gọn</span
                  > -->
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
                        slot="title"
                        @click="openEditComment(item)"
                        class="submit-btn"
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
                <textarea
                  v-if="isRely && item.replyOfItem && isMain"
                  placeholder="Nhập trả lời......."
                  :class="{
                    boxReply: item.subComment,
                    nBoxReply: !item.subComment,
                  }"
                  class="text-area"
                  rows="1"
                  @keydown.enter.exact.prevent="
                    saveSubComment(item.id, item.subComment)
                  "
                  @keydown.enter.shift.exact.prevent="item.subComment += '\n'"
                  @keydown.enter.ctrl.exact.prevent="item.subComment += '\n'"
                  v-model="item.subComment"
                />
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
                  v-if="item.subComment && element.isReplySub"
                  @click="saveSubComment(item.id, element.subComment)"
                  size="medium"
                  class="btn-save-main-comment mt-2 mr-btn-2 f-right"
                  type="primary"
                >
                  <h4 class="sending-button">{{ $t("common.send") }}</h4>
                </Button>
                <p
                  class="show-sub-feebback"
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
        <div v-if="item.isRespone" class="domain-reply-comment">
          <Row
            v-for="element in item.replyComment"
            :key="element.id"
            class="ml-15"
          >
            <Col span="3" class="img-reply">
              <div>
                <img :src="element.avatar" class="image-avatar-reply" /></div
            ></Col>
            <Col span="21">
              <div class="mgl-2">
                <span class="text-username" @click="openBlog(element.userId)"
                  ><b>{{ element.userName }}</b></span
                >
                <br />
                <pre v-if="!element.isShow" class="text-content-comment">{{
                  element.comment
                }}</pre>
                <textarea
                  v-if="element.isShow"
                  placeholder="Viết bình luận"
                  class="text-area edit-box"
                  rows="1"
                  @keydown.enter.exact.prevent="saveComment"
                  @keydown.enter.shift.exact.prevent="
                    element.editComment += '\n'
                  "
                  @keydown.enter.ctrl.exact.prevent="
                    element.editComment += '\n'
                  "
                  v-model="element.editComment"
                />
                <div v-if="element.isShow" class="f-right">
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
                    :disabled="isActive && !element.isReplySub && isRelyChild"
                    v-bind:class="{
                      noReply: isActive && !element.isReplySub && isRelyChild,
                    }"
                    v-if="!isRelyChild"
                    @click="showInputChildRely(element, item, 'Reply')"
                  >
                    Trả lời
                  </button>
                  <button
                    class="action-icon reply-btn"
                    v-else
                    :disabled="isActive && !element.isReplySub && isRelyChild"
                    v-bind:class="{
                      noReply: isActive && !element.isReplySub && isRelyChild,
                    }"
                    @click="showInputChildRely(element, item, 'NoReply')"
                  >
                    Trả lời
                  </button>
                  <span class="before-time mr-20"
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
                <textarea
                  v-if="isRelyChild && element.isReplySub"
                  placeholder="Nhập trả lời......."
                  :class="{
                    boxReply: element.subComment,
                    nBoxReply: !element.subComment,
                  }"
                  class="text-area"
                  @keydown.enter.exact.prevent="
                    saveSubComment(item.id, element.subComment)
                  "
                  @keydown.enter.shift.exact.prevent="
                    element.subComment += '\n'
                  "
                  @keydown.enter.ctrl.exact.prevent="element.subComment += '\n'"
                  rows="1"
                  v-model="element.subComment"
                />
                <Button
                  style="float: right"
                  v-if="element.subComment && element.isReplySub"
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
                  v-if="element.subComment && element.isReplySub"
                  @click="saveSubComment(item.id, element.subComment)"
                  size="medium"
                  class="btn-save-main-comment mt-2 mr-btn-2 f-right"
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
        <Col v-if="isSeeMore" span="16" class="row-see-more">
          <span @click="seeMore()">
            <strong>Tải thêm bình luận .... </strong></span
          >
        </Col>
      </Row>
    </div>
    <div slot="footer">
      <div>
        <h4 class="h4-space"></h4>
        <Mentionable
          class="comment"
          :keys="['@']"
          :items="[]"
          :limit="5"
          :mapInsert="mapInsert"
          offset="6"
        >
          <span class="avatar avatar-reply-comment"><img :src="Avartar"/></span>
          <textarea
            placeholder="Viết bình luận......."
            class="text-area box-reply-main"
            :class="{ send: comment, nsend: !comment }"
            rows="1"
            @keydown.enter.exact.prevent="saveMainComment(data.id)"
            @keydown.enter.shift.exact.prevent="comment += '\n'"
            @keydown.enter.ctrl.exact.prevent="comment += '\n'"
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
                src="../images/smile.png"
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
            @click="saveMainComment(data.id)"
            size="small"
            class="btn-save-main-comment send-comment"
            type="primary"
            v-if="comment"
          >
            <!-- <i class="fa fa-paper-plane"></i> -->
            <img width="30" src="../images/send.png" />
          </Button>
        </Mentionable>
      </div>
    </div>
  </Modal>
</template>

<script lang="ts">
import DashboardDto from "@/store/entities/dashboard";
import { EStatus } from "@/types/information/EStatus";
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import url from "../lib/url";
import { Mentionable } from "vue-mention";
import moment from "moment";

@Component({
  components: { Mentionable },
  props: {},
  filters: {
    moment(value: any) {
      return moment(value).format("HH:mm YYYY-MM-DD");
    },
  },
})
export default class DialogComment extends Vue {
  comment: string = "";
  @Prop() private data: DashboardDto;
  private order: number = 1;
  private pageSizeComment: number = 5;
  private isSeeMore: boolean = false;
  private isActive: boolean = false;
  private news: any = {};
  private search: string = "";
  private isMain = true;
  private isShowTitle: boolean = true;
  private listFace = [];
  private EStatus: any = EStatus;
  private isNew: boolean = false;
  private dataComment: any = [];
  private isSubComment: boolean = false;
  private dataEditComment: any = {};
  private dataDefaultComment: string;
  private entityId: string = "";
  private userId: number = null;
  private isAdmin: boolean = false;
  private items: any = [{ label: "a" }, { label: "b" }];
  private userIds: any = [];
  private isFeedback = false;
  private isRely = false;
  private isRelyChild = false;
  public lastEle: any;
  created() {
    this.fetchData();
  }

  insert(emoji) {
    this.comment += emoji;
  }

  private showTitle() {
    this.isShowTitle = !this.isShowTitle;
  }

  private mapInsert(item) {
    return "(" + item.label + ")";
  }
  private getUserInComment(data: string) {
    let comment = data;
    let elComment: any = [];
    let commentData: any = {
      editComment: "",
      elComment: [],
    };
    while (comment.indexOf("-)") >= 0) {
      elComment.push(comment.slice(0, comment.indexOf("-)") + 2));
      comment = comment.slice(comment.indexOf("-)") + 2);
    }
    elComment.push(comment);
    elComment = elComment.map((el) => {
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
    commentData.elComment = elComment;
    return commentData;
  }
  private fixComment(data: string) {
    let comment = data;
    let elComment: any = [];
    let newComment = "";
    this.userIds = [];
    while (comment.indexOf(")") >= 0) {
      elComment.push(comment.slice(0, comment.indexOf(")") + 1));
      comment = comment.slice(comment.indexOf(")") + 1);
      // do something
    }
    elComment.push(comment);
    elComment.map((el) => {
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
    //this.getAllNews();
    this.getComment();
    if (this.$route.params.s) {
      this.news = await this.$store.dispatch({
        type: "news/getNews",
        id: this.$route.params.newsId,
      });
      this.news.image = url + this.news.image;
    }
  }
  // beforeMount() {
  //   this.fetchData();
  // }

  // backToList() {
  //   this.$router.push("/information/news");
  // }
  seeMoreNews() {
    this.$router.push("/information/news");
  }
  showInputRely(item, string) {
    this.isActive = !this.isActive;
    if (string === "Reply") {
      item.replyOfItem = true;
      this.isRely = !this.isRely;
    } else {
      item.replyOfItem = false;
      this.isRely = !this.isRely;
    }
  }

  forChooseRightItem(item) {}
  showInputChildRely(element, item, string) {
    this.isActive = !this.isActive;
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

  handleEditNew() {
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
    // this.dataComment = [];
    if (status) {
      this.isNew = !this.isNew;
      if (status === "new") {
        this.order = 1;
      } else this.order = 2;
    }
    let params = {
      entityId: this.data.id,
      entityName: this.data.entityName,
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

    let temp = mainComment.items.map((el) => {
      let newEl = el;
      newEl.newComment = el.comment
        ? this.getUserInComment(el.comment).el_comment
        : "";
      newEl.editComment = el.comment
        ? this.getUserInComment(el.comment).editComment
        : "";
      if (el.avatar) {
        newEl.avatar = el.avatar.includes("http") ? el.avatar : url + el.avatar;
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
    if (this.dataComment.length > 0) {
      for (let i = 0; i < temp.length; i++) {
        if (!this.dataComment[i]) {
          this.dataComment.push(temp[i]);
        }
      }
    } else {
      this.dataComment.push(...temp);
    }
    this.dataComment.forEach((element) => {
      this.getSubComment(element.id);
    });
  }
  private cancelReplySubComment(item: any, mainComment: any) {
    this.isMain = !this.isMain;
    mainComment.subComment = "";
    item.subComment = "";
    this.isActive = false;
    item.isReplySub = false;
    this.isRelyChild = !this.isRelyChild;
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

  private async saveMainComment(id: number) {
    this.dataComment = [];
    if (this.comment) {
      this.isActive = false;
      const newComment = this.fixComment(this.comment);
      let params = {
        entityId: id,
        entityName: this.data.entityName,
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
      this.data.totalComment++;
    }
  }

  private async saveSubComment(id: number, childComment?: string) {
    if (childComment) {
      let subComment = "";
      this.isActive = false;
      this.isRelyChild = !this.isRelyChild;
      this.isMain = !this.isMain;
      this.dataComment.forEach((element) => {
        if (element.id === id) {
          subComment = this.fixComment(childComment);
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
      this.dataComment = [];
      this.getComment();
      this.data.totalComment++;
    }
  }
  private openEditComment(item: any, isSub?: string) {
    if (!isSub) {
      this.isSubComment = false;
    } else {
      this.isSubComment = true;
    }
    this.dataEditComment = item;
    this.dataDefaultComment = item.comment;
    item.visible = !item.visible;
    item.isShow = !item.isShow;
    // this.entityId = this.$route.params.newsId;
    this.entityId = item.id;
    this.$forceUpdate();
  }
  private cancelReplyMainComment(item: any) {
    this.isRely = !this.isRely;
    this.isActive = false;
    item.subComment = "";
    item.replyOfItem = false;
  }

  private async activeDeleteComment(item: any, isSub?: string) {
    if (!isSub) {
      await this.$store.dispatch({
        type: "news/deleteMainComment",
        idComment: item.id,
      });
      let amount = item.totalReply + 1;
      while (amount) {
        this.data.totalComment--;
        amount--;
      }
      this.getComment();
    } else {
      await this.$store.dispatch({
        type: "news/deleteSubComment",
        idSubComment: item.id,
      });
      this.data.totalComment--;
      this.getComment();
    }
  }

  private async deleteComment(item: any, isSub?: string) {
    item.visible = !item.visible;
    this.$fire({
      title: "Are you sure?",
      text: "Delete this comment?",
      showCancelButton: true,
      confirmButtonColor: "#d33",
      cancelButtonColor: "#3085d6",
      confirmButtonText: "Yes, delete it!",
    }).then((r) => {
      if (r.value) {
        this.dataComment = [];
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

  async saveComment() {
    this.dataComment = [];
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
        entityName: this.data.entityName,
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
    this.data.totalComment++;
  }

  private cancel(item: any) {
    item.isShow = !item.isShow;
    item.editComment = this.dataDefaultComment;
    this.$forceUpdate();
  }
  // cancel(item: any, isSub?: string) {
  //   item.isShow = !item.isShow;
  //   item.comment = this.dataDefaultComment.comment;
  //   this.$forceUpdate();
  // }

  openBlog(id: any) {
    this.$router.push({ name: "userBlog", params: { userId: id } });
  }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
* {
  color: #333;
}
p {
  text-transform: uppercase;
  font-size: 18px;
  font-weight: bold;
  padding: 5px 0;
}
.btn-zone {
  float: right;
}
.titleDialogHide {
  word-break: break-word !important;
  overflow: hidden;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  white-space: pre-wrap;
  line-height: 25px;
}
.titleDialogShow {
  word-break: break-word !important;
  line-height: 25px;
  display: block;
}
.action-comment {
  margin-top: 10px;
  /* margin-bottom: 20px; */
  font-size: 13px;
  display: flex;
  align-items: center;
  .action-icon {
    margin-right: 10px;
    cursor: pointer;
    display: flex;
    align-items: center;
  }
}
.info-header {
  width: fit-content;
  align-items: center;
  /* justify-content: space-between; */
  gap: 10px;
  div {
    margin: 0px 3px;
    width: max-content;
    :first-child {
      margin: 0;
    }
    :last-child {
      margin: 0;
    }
  }
  p {
    padding: 5px;
    font-size: 12px;
    line-height: 10px;
    height: auto;
    :first-child {
      display: flex;
      gap: 10px;
    }
  }
}
.btn-save-main-comment {
  color: #ffffff;
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
    z-index: 99;
    position: absolute;
    right: 20px;
    font-size: 32px;
    margin-top: 5px;
  }
}
.btt-see-more {
  margin-top: 30px;
}
.show-sub-feebback {
  color: #757575;
  span {
    color: #757575;
  }
}
.box-header {
  flex-direction: column;
  align-items: flex-start;
  gap: 5px;
}
.alg-center {
  display: flex;
  align-items: center;
  gap: 3px;
}
.bg-date {
  background: #faebd7;
}
.reply-btn {
  font-family: "Segoe UI", "sans-serif";
  color: #757575;
  font-size: 16px;
  border: none;
  background: transparent;
  height: 20px;
  margin-top: -4.8px;
  :hover {
    background: #ffff;
  }
}
.bg-cmt {
  background: #faebd7;
  float: left;
}
.bg-like {
  background: #faebd7;
  float: left;
}
.noReply {
  cursor: not-allowed !important;
}
.before-time {
  margin-top: -2px !important;
}
.boxReply {
  border: 1px solid #f1f1f1 !important;
  width: 100%;
  border-radius: 10px;
}
.nBoxReply {
  border-radius: 10px;
  border: 1px solid #f1f1f1 !important;
  width: 100%;
}
.domain-reply-comment {
  margin-top: 10px;
}
.send-comment {
  padding: 0 !important;
  border: none;
  background: #ffffff;
}
.comment-zone {
  margin-top: 7px;
  padding: 1px 15px;
  border-radius: 12px;
}
.mr-btn-2 {
  margin-right: 5px;
}
.send {
  width: 73%;
}
.nsend {
  width: 80%;
}

.box-reply {
  width: 100%;
  border: 1px solid #f1f1f1;
  border-radius: 10px;
}
.un-expand {
  margin-bottom: -2.5px;
}
.box-reply-main {
  border: 1px solid #f1f1f1;
  border-radius: 10px;
}
.cancel-button {
  color: #000000;
  font-size: 14px;
  font-weight: 700;
}
.text-area {
  padding: 7px;
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
  outline: none;
  /* border: none; */
}
.edit-box {
  border: 1px solid #f1f1f1 !important;
  border-radius: 20px;
  width: 100%;
}
.text-content-comment {
  padding: 0;
  margin: 0;
  white-space: pre-line;
  word-break: break-word;
  color: #000000;
  font-size: 15px;
  background: #ffff;
  border: none;
  font-family: sans-serif;
}
.submit-btn:hover {
  cursor: pointer;
  a {
    color: #2b85e4;
  }
}
.btn-save {
  color: #ffffff;
  margin-right: 5px;
}
.delete-btn:hover {
  cursor: pointer;
  a {
    color: #ec2323;
  }
}
.img-reply {
  width: 10%;
}
.ivu-icon-md-more {
  margin-top: -3px;
  cursor: pointer;
}
.mr-2 {
  margin-right: 10px;
}
.avatar-reply-comment {
  width: 40px;
}
.text-content-comment {
  padding: 0;
  margin: 0;
  white-space: pre-line;
  word-break: break-word;
  color: #000000;
  font-size: 15px;
  background: #ffff;
  border: none;
  font-family: sans-serif;
}
.img-reply {
  width: 10%;
}
.ivu-icon-md-more {
  margin-top: -3px;
  cursor: pointer;
}
.mr-2 {
  margin-right: 10px;
}
.avatar-reply-comment {
  width: 40px;
}
.lastestNew {
  font-size: 16px;
  margin-top: 50px;
  margin-right: 15px;
  cursor: pointer;
}
.comment:nth-child(2) {
  display: flex;
  align-items: center;
  border-radius: 3px;
  position: relative;
  padding: 10px 0 10px 0;
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
.width_common {
  width: 100%;
  float: left;
}
.sending-button {
  color: #fff;
  font-size: 14px;
  font-weight: 700;
}
.action-comment span,
.action-comment span i {
  color: rgb(117, 117, 117);
}
.action-comment span {
  margin-top: -5px;
}
.action-comment span:nth-child(1) i {
  margin-left: 5px;
}
@media only screen and (max-width: 968px) {
  .content-news {
    width: 100%;
    float: right;
  }
}
@media only screen and (max-width: 415px) {
  .body-content {
    padding: 16px;
  }
  .comment-wrap {
    width: 100% !important;
  }
}
.modal-mask {
  position: fixed;
  z-index: 9998;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: table;
  transition: opacity 0.3s ease;
}

.modal-wrapper {
  display: table-cell;
  vertical-align: middle;
}

.modal-container {
  width: 300px;
  margin: 0px auto;
  padding: 20px 30px;
  background-color: #fff;
  border-radius: 2px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33);
  transition: all 0.3s ease;
  font-family: Helvetica, Arial, sans-serif;
}

.modal-header h3 {
  margin-top: 0;
  color: #42b983;
}

.modal-body {
  margin: 20px 0;
}

.modal-default-button {
  display: block;
  margin-top: 1rem;
}

.modal-enter {
  opacity: 0;
}

.total-cmt {
  background: #f1f1f1;
}

.modal-leave-active {
  opacity: 0;
}

.modal-enter .modal-container,
.modal-leave-active .modal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}
</style>
<style>
.list-item {
  height: 100%;
}
.list-item .title-post {
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
}
.dashboard .lastest-news {
  margin-bottom: 0px !important;
}
.ivu-modal {
  top: 15vh;
}
.modal-comment .ivu-modal-content {
  border-radius: 20px !important;
}
.ivu-modal-footer {
  border-top: 1px solid #e8eaec;
  padding: 0 !important;
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
.boder {
  margin-top: 100px;
  border-width: 1px;
  border-bottom: 1px solid gray;
}

.emoji-config {
  position: absolute;
  background-color: hsla(0, 0%, 100%, 0.98);
  display: block;
  height: 210px;
  right: 0;
  width: 180px;
  border-radius: 2%;
  top: -215px;
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
  margin-top: 5px;
  z-index: 99;
}
.list-emoji {
  height: 175px;
  width: 178px;
  overflow-x: hidden;
  overflow-y: auto;
}
/* width */
.list-emoji::-webkit-scrollbar {
  width: 5px;
}

/* Track */
.list-emoji::-webkit-scrollbar-track {
  background: #f1f1f1;
}

/* Handle */
.list-emoji::-webkit-scrollbar-thumb {
  background: #888;
}

/* Handle on hover */
.list-emoji::-webkit-scrollbar-thumb:hover {
  background: #555;
}
.body-list-comment {
  padding-top: 10px;
  height: 350px;
  overflow-y: auto;
  overflow-x: hidden;
}
.body-list-comment::-webkit-scrollbar {
  width: 7px;
}
.body-list-comment::-webkit-scrollbar-track {
  background: #f1f1f1;
  border-radius: 10px;
}

/* Handle */
.body-list-comment::-webkit-scrollbar-thumb {
  background: #888;
  border-radius: 10px;
}

/* Handle on hover */
.body-list-comment::-webkit-scrollbar-thumb:hover {
  background: #555;
}
.ivu-modal-header {
  padding: 25px 40px 5px !important;
}
.ivu-modal-close {
  top: 6px !important;
}
.ivu-modal-body {
  padding: 8px 16px !important;
}
.single-post {
  height: auto;
}
@media only screen and (max-width: 968px) {
  .ivu-modal {
    top: 15vh;
  }
}
::v-deep .swal2-container.swal2-center.swal2-shown {
  background-color: green;
  z-index: 1000000 !important;
}
::v-deep .ivu-modal-mask {
  z-index: 1000 !important;
}
::v-deep .ivu-modal-wrap {
  z-index: 1001 !important;
}
</style>
