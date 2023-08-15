<template>
  <div class="list-item">
    <card v-if="data" class="single-post">
      <!-- <Poptip
        class="poptip-menu"
      >
        <i
          data-v-7f89a61e=""
          class="ivu-icon ivu-icon-md-more"
        ></i>
        <div
          slot="title"
          class="submit-btn"
          v-if="
            this.event.status == EStatus.Draft &&
            SessionStore.state.user.canWaitingFromDraft
          "
          @click="changeStatus(EStatus.WaitingForApproval)"
        >
          <a>{{ $t("common.submit") }}</a>
        </div>
        <div
          slot="title"
          class="submit-btn"
          v-if="
            this.event.status == EStatus.Return &&
            SessionStore.state.user.canWaitingFromReturn
          "
          @click="changeStatus(EStatus.WaitingForApproval)"
        >
          <a>{{ $t("common.submitAgain") }}</a>
        </div>
        <div
          slot="title"
          class="submit-btn"
          v-if="
            this.event.status == EStatus.WaitingForApproval &&
            SessionStore.state.user.canAppoveFromWaiting
          "
          @click="changeStatus(EStatus.Approved)"
        >
          <a>{{ $t("common.approve") }}</a>
        </div>
        <div
          slot="title"
          class="submit-btn"
          v-if="
            this.event.status == EStatus.Return &&
            SessionStore.state.user.canAppoveFromReturn
          "
          @click="changeStatus(EStatus.Approved)"
        >
          <a>{{ $t("common.approve") }}</a>
        </div>
        <div
          slot="title"
          class="submit-btn"
          v-if="
            this.event.status == EStatus.WaitingForApproval &&
            SessionStore.state.user.canReturnFromWaiting
          "
          @click="changeStatus(EStatus.Return)"
        >
          <a>{{ $t("common.return") }}</a>
        </div>
        <div
          slot="title"
          class="submit-btn"
          v-if="
            this.event.status == EStatus.Approved &&
            SessionStore.state.user.canHiddenFromApprove
          "
          @click="changeStatus(EStatus.Hidden)"
        >
          <a>{{ $t("common.hidden") }}</a>
        </div>
        <div
          slot="title"
          class="submit-btn"
          v-if="
            this.event.status == EStatus.Approved &&
            SessionStore.state.user.canReturnFromApprove
          "
          @click="changeStatus(EStatus.Disabled)"
        >
          <a>{{ $t("common.hidden") }}</a>
        </div>
        <div
          slot="title"
        >
          <a>{{ $t("common.edit") }}</a>
        </div>
      </Poptip> -->
      <div class="tooltip" @click="viewDetail(data, $event)">
        <div class="title-post" @click="viewDetail(data, $event)">
          {{ data.title }}
        </div>
        <span class="tooltiptext">{{ data.title }}</span>
      </div>
      <div class="box" @click="viewDetail(data, $event)">
        <img
          v-if="data.image != null"
          class="img-item"
          :src="data.image"
          alt="img"
        />
        <img
          v-else
          class="img-item"
          src="../images/image-gallery.png"
          alt="img"
        />
      </div>
      <div class="footer-post">
        <div class="footer-left">
          <div class="like" @click="actionLike(data)">
            <img
              class="action-btn"
              v-if="!data.userLike.isLiked"
              src="../images/Like.svg"
            />
            <img class="action-btn" v-else src="../images/Liked.svg" />
            <div class="amount" v-bind:class="{ liked: data.userLike.isLiked }">
              {{ data.userLike.totalLike }}
            </div>
          </div>
          <button
            type="button"
            class="btn-show-dialog-comment"
            @click="showModal(data.id)"
          >
            <div class="total-comment">{{ data.totalComment }}</div>
          </button>
          <div></div>
          <div class="text">
            <DialogComment
              :data="data"
              v-if="isOpenBoxComment"
              ref="childComponent"
            ></DialogComment>
          </div>
        </div>
        <div class="createDate">{{ data.createDate | moment }}</div>
      </div>
    </card>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import moment from "moment";
import SessionStore from "../store/modules/session";
import { EStatus } from "../types/information/EStatus";
import DialogComment from "./dialog-comment.vue";
import { InforEntity } from "@/store/entities/inforEntity";
import DashboardDto from "@/store/entities/dashboard";
import FundAmountHistory from "./fund-amount-history.vue";
@Component({
  components: { DialogComment, FundAmountHistory },
  filters: {
    moment(value: any) {
      return moment(value).format("DD-MM-YYYY HH:mm");
    },
  },
})
export default class ListItem extends Vue {
  @Prop() private data: DashboardDto;
  private EStatus = EStatus;
  private SessionStore = SessionStore;
  private isOpenBoxComment: boolean = false;

  showModal(id: number) {
    // this.$router.push(`/dashboard/${id}`);
    this.isOpenBoxComment = true;
    this.data.isShow = !this.data.isShow;
    this.$emit("changeIsShow", this.data);
    // this.$emit("getAllComment");
  }

  private updateLike(item: any) {
    if (this.data.userLike.isLiked) {
      item.userLike.totalLike++;
    } else {
      item.userLike.totalLike--;
    }
  }

  // private async getTotalLike(item: any) {
  //   switch (item.entityName) {
  //     case InforEntity.New:
  //       let newest = await this.$store.dispatch({
  //         type: "news/getTotalLike",
  //         id: item.id,
  //       });
  //       item.userLike.totalLike = newest.totalLike;
  //       break;
  //     case InforEntity.News:
  //       let news = await this.$store.dispatch({
  //         type: "news/getTotalLike",
  //         id: item.id,
  //       });
  //       item.userLike.totalLike = news.totalLike;
  //       break;
  //     case InforEntity.Policy:
  //       let policy = await this.$store.dispatch({
  //         type: "policy/getTotalLike",
  //         id: item.id,
  //       });
  //       item.userLike.totalLike = policy.totalLike;
  //       break;
  //     case InforEntity.GuideLine:
  //       let guideLine = await this.$store.dispatch({
  //         type: "guide/getTotalLike",
  //         id: item.id,
  //       });
  //       item.userLike.totalLike = guideLine.totalLike;
  //       break;
  //     case InforEntity.Event:
  //       let event = await this.$store.dispatch({
  //         type: "event/getTotalLike",
  //         id: item.id,
  //       });
  //       item.userLike.totalLike = event.totalLike;
  //       break;
  //     default:
  //       break;
  //   }
  // }

  private async actionLike(item: any) {
    switch (item.entityName) {
      case InforEntity.New:
        let newest = await this.$store.dispatch({
          type: "news/sendLikePost",
          id: item.id,
        });
        item.userLike.isLiked = newest.isLiked;
        this.updateLike(item);
        break;
      case InforEntity.News:
        let news = await this.$store.dispatch({
          type: "news/sendLikePost",
          id: item.id,
        });
        item.userLike.isLiked = news.isLiked;
        this.updateLike(item);
        break;
      case InforEntity.Policy:
        let policy = await this.$store.dispatch({
          type: "policy/sendLikePost",
          id: item.id,
        });
        item.userLike.isLiked = policy.isLiked;
        this.updateLike(item);
        break;
      case InforEntity.GuideLine:
        let guideLine = await this.$store.dispatch({
          type: "guide/sendLikePost",
          id: item.id,
        });
        item.userLike.isLiked = guideLine.isLiked;
        this.updateLike(item);
        break;
      case InforEntity.Event:
        let event = await this.$store.dispatch({
          type: "event/sendLikePost",
          id: item.id,
        });
        item.userLike.isLiked = event.isLiked;
        this.updateLike(item);
        break;
      default:
        break;
    }
  }
  // actionLike() {
  //   if (this.isLiked == false) {
  //     this.amount++;
  //   } else {
  //     this.amount--;
  //   }
  //   this.isLiked = !this.isLiked;
  // }
  viewDetail(item: any, e) {
    let route;
    switch (item.entityName) {
      case InforEntity.New:
        route = this.$router.resolve({
          name: "NewsDetail",
          params: { newsId: item.id },
        });
        break;
      case InforEntity.News:
        route = this.$router.resolve({
          name: "NewsDetail",
          params: { newsId: item.id },
        });
        break;
      case InforEntity.NCC8:
        route = this.$router.resolve({
          name: "NewsDetail",
          params: { newsId: item.id },
        });
        break;
      case InforEntity.NewStaff:
        route = this.$router.resolve({
          name: "NewsDetail",
          params: { newsId: item.id },
        });
        break;
      case InforEntity.Policy:
        route = this.$router.resolve({
          name: "policyDetail",
          params: { policyId: item.id },
        });
        break;
      case InforEntity.GuideLine:
        route = this.$router.resolve({
          name: "GuideDetail",
          params: { guideId: item.id },
        });
        break;
      case InforEntity.Event:
        route = this.$router.resolve({
          name: "EventDetail",
          params: { eventId: item.id },
        });
        // this.$router.push(`/information/events/${item.id}`);
        break;
      default:
        //   route = this.$router.resolve({
        //     name: "EventDetail",
        //     params: { eventId: item.id },
        //   });
        break;
    }
    if (e.ctrlKey) {
      window.open(route.href, "_blank");
    } else {
      window.open(route.href, "_self");
    }
  }
}
</script>
<style lang="scss" scoped>
@import url("https://fonts.googleapis.com/css2?family=Oswald&display=swap");
@import "src/assets/style/_variable.scss";
.box {
  display: flex;
  justify-content: center;
  align-items: center;
  img {
    height: 100%;
    width: 100%;
  }
}
.poptip-menu {
  position: absolute;
  font-size: 20px;
  right: 10px;
  top: 5px;
  height: 25px;
  width: 13px;
  z-index: 99;
}
.liked {
  color: #0068ca;
  font-weight: 700;
}
.list-item {
  cursor: pointer;
  z-index: 1;
  img {
    object-position: center;
    background-color: black;
  }
  .box {
    width: 100%;
    height: 10vw;
    float: left;
    position: relative;
    overflow: hidden;
    img {
      object-fit: cover;
    }
  }

  .tag {
    max-width: 50px;
    position: absolute;
  }

  .tag-info {
    position: absolute;
    top: 40%;
    left: 50%;
    -webkit-transform: translate(-50%, -50%);
    transform: translate(-50%, -50%);
    color: #fff;
    font-size: 10px;
    text-align: center;
    width: 100%;
    overflow: hidden;
  }
  .footer-post {
    display: flex;
    padding: 10px 11px 10px 20px;
    align-items: flex-end;
    justify-content: space-between;
  }
  .createDate {
    color: $color-gray-light-8;
    font-size: 15px;
  }
  ::v-deep .ivu-card-bordered {
    border: none;
  }
  .description,
  .title-post {
    position: relative;
    height: 70px;
    padding: 14px 15px;
    line-height: 26px;
    font-size: 1.8rem;
    font-weight: 600;
    color: #000000;
    /* font-family: Oswald;  */
    overflow: hidden;
    text-overflow: ellipsis;
    display: -webkit-box;
    word-break: break-word;
    -webkit-line-clamp: 2;
    -webkit-box-orient: vertical;
    text-transform: uppercase;
    &:hover {
      text-decoration: none;
      color: $color-red-light;
    }
  }
  .description {
    font-size: 15px;
  }
  .list-item-right {
    padding-left: 15px;
    span {
      font-size: 12px;
      font-style: italic;
      margin-top: 20px;
    }
    h3 {
      display: flex;
      align-items: center;
      .type-tag {
        color: red;
        display: inline-block;
        border: 1px solid red;
        border-radius: 10px;
        padding: 0 5px;
        font-size: 10px;
        margin-left: 10px;
      }
    }
    p {
      height: 85px;
      overflow: hidden;
      text-overflow: ellipsis;
    }
  }
}
.footer-left {
  display: flex;
}
.tooltip {
  z-index: unset;
  position: relative;
  display: inline-block;
}

.tooltip .tooltiptext {
  visibility: hidden;
  width: fit-content;
  background-color: black;
  color: #fff;
  text-align: center;
  border-radius: 6px;
  padding: 5px;
  position: absolute;
  z-index: 99;
  top: 110%;
}
.tooltip .tooltiptext::after {
  content: "";
  position: absolute;
  bottom: 100%;
  left: 50%;
  margin-left: -5px;
  border-width: 5px;
  border-style: solid;
  border-color: transparent transparent black transparent;
}

.tooltip:hover .tooltiptext {
  visibility: visible;
  transition-delay: 0.8s;
}
.like {
  padding-right: 20px;
  display: flex;
  img {
    width: 24px;
    background-color: transparent;
  }
  .amount {
    width: 10px;
    font-size: 18px;
    padding-top: 0.3vw;
    padding-left: 5px;
  }
}
.total-comment {
  font-size: 18px;
  padding-left: 33px;
}
.single-post {
  position: relative;
  background: #ffffff 0% 0% no-repeat padding-box;
  box-shadow: 0 0 1.2rem 0 rgba(0 0 0 / 0.15);
  border: 1px solid #dddddd;
  border-radius: 7px;
  padding-left: 0px;
  padding-top: 0px;
  margin-right: 0px;
}

::v-deep .ivu-card-body {
  background-color: transparent;
  padding: 0px;
  display: flex;
  flex-direction: column;
  border-radius: 7px;
}
.list-item-detail {
  height: 150px;
  overflow: hidden;
}
@media only screen and (min-width: 1980px) {
  .title-post {
    height: 100px !important;
    font-size: 31px !important;
    line-height: 40px !important;
  }
}
@media only screen and (max-width: 1500px) {
  .like {
    padding-right: 1.2rem;
    .amount {
      font-size: 1.3rem;
    }
  }
  .total-comment {
    font-size: 1.3rem;
    padding-left: 2.8rem;
  }
  .createDate {
    font-size: 1.3rem !important;
  }
}
@media only screen and (max-width: 1400px) {
  .box-news {
    padding-left: 0px !important;
    padding-right: 0px !important;
    .ivu-card-body {
      height: 240px;
      border-radius: 5px;
    }
  }
  .title-post {
    font-size: 15px !important;
    height: 70px !important;
  }
  .box {
    height: 13vw !important;
  }
  .like {
    .amount {
      font-size: 1.4rem;
    }
  }
  .total-comment {
    font-size: 1.4rem;
    padding-left: 2.8rem;
  }
  .btn-show-dialog-comment {
    margin-top: 4px;
  }
  .like {
    padding-right: 1.2rem;
  }
  .createDate {
    font-size: 1.4rem !important;
  }
}

@media only screen and (max-width: 1150px) {
  .title-post {
    padding: 10px 11px !important;
    font-size: 15px !important;
    height: 53px !important;
    line-height: 20px !important;
  }
  .box {
    height: 20vw !important;
  }
}

@media only screen and (max-width: 960px) {
  .like {
    padding-right: 2px;
    .action-btn {
      width: 50% !important;
    }
    .amount {
      line-height: 22px;
      padding-top: 0.42vw;
    }
  }
  .btn-show-dialog-comment {
    width: 15px;
    .total-comment {
      font-size: 14px;
      padding-left: 20px;
    }
  }
}

@media only screen and (max-width: 900px) {
  .like {
    padding-right: 0.5rem;
  }
}

@media only screen and (max-width: 768px) {
  .list-item {
    .box {
      height: 27vw !important;
    }
  }
}

@media only screen and (max-width: 518px) {
  .list-item {
    .box {
      width: 100% !important;
      height: 205px !important;
      margin-right: 0 !important;
    }
  }
  .img-item {
    max-height: 200px !important;
  }
  .title-post {
    font-size: 20px !important;
    line-height: 24px !important;
    height: 60px !important;
  }
  .description {
    text-align: justify;
    -webkit-line-clamp: 3 !important;
    font-size: 17px !important;
  }
  .footer-left {
    .like {
      padding-right: 20px;
      .action-btn {
        width: 65% !important;
      }
      .amount {
        font-size: 15px;
        line-height: 26px;
      }
    }
  }
  .btn-show-dialog-comment {
    margin-top: 4px;
    width: 20px;
  }
  .footer-post {
    .createDate {
      font-size: 14px !important;
    }
  }
  .like {
    padding-right: 10px;
  }
  .footer-left .btn-show-dialog-comment .total-comment {
    font-size: 15px;
    padding-left: 25px;
  }
}
</style>
