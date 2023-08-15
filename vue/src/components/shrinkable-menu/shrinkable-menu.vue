<template>
  <div class="ivu-shrinkable-menu">
    <!-- <slot name="top"></slot> -->
    <sidebar-menu
      v-show="!shrink"
      :menu-theme="theme"
      :menu-list="menuList"
      :open-names="openNames"
      @on-change="handleChange"
    ></sidebar-menu>
    <sidebar-menu-shrink
      v-show="shrink"
      :menu-theme="theme"
      :menu-list="menuList"
      :icon-color="shrinkIconColor"
      @on-change="handleChange"
    ></sidebar-menu-shrink>

    <navigation :menu-list="menuList" @on-change="handleChange"></navigation>
    <!-- <div class="notification"> -->
    <!-- <Poptip v-model="isNotification" @on-popper-show="getAllNotification" @on-popper-hide="clickNotification"> -->
    <!-- <div class="icon">
             <i class="ivu-icon icon ivu-icon-ios-notifications-outline" @click="clickNotification" ></i>
            <span v-if="notification" align="center" class="notification-number">{{notification}}</span>
         </div>
            <div slot="title" v-if="isNotification" class="notification-list">
                <div class="notification-item" v-for="item in listNotification" :key="item.id">
                    <div class="notification-item"  @click="viewDetail(item)">
                        <img :src="item.authorAvatar"/>
                        <div class="messsage">{{ item.messsage }}</div>
                    </div>
                </div>
            </div> -->
    <!-- </Poptip> -->
    <!-- </div> -->
  </div>
</template>

<script lang="ts">
import sidebarMenu from "./components/sidebarMenu.vue";
import sidebarMenuShrink from "./components/sidebarMenuShrink.vue";
import navigation from "./components/menu-mobile/navigation.vue";
import util from "../../lib/util";
import { Component, Vue, Inject, Prop, Emit } from "vue-property-decorator";
import { InforEntity } from "../../store/entities/inforEntity";
import url from "../../lib/url";
@Component({
  components: { sidebarMenu, sidebarMenuShrink, navigation },
})
export default class ShrinkableMenu extends Vue {
  name: string = "shrinkableMenu";
  @Prop() shrink: boolean;
  @Prop({ required: true, type: Array }) menuList: Array<any>;
  @Prop({ type: Array }) openNames: Array<string>;
  @Prop({ type: Function }) beforePush: Function;
  theme = "light";
  isNotification: boolean = false;
  private listNotification: any = [];
  private InforEntity = InforEntity;
  private notification: number = 0;
  get bgColor() {
    return this.theme === "dark" ? "#001529" : "#f0f2f5";
  }
  get shrinkIconColor() {
    return this.theme === "dark" ? "#fff" : "#495060";
  }
  @Emit("on-change")
  handleChange(name: string) {
    let willpush = true;
    if (this.beforePush !== undefined) {
      if (!this.beforePush(name)) {
        willpush = false;
      }
    }
    if (willpush && name !== "Dashboard" && name !== "UserCalendar") {
      this.$router.push({ name: name }).catch(() => {});
    }
  }

  private async created() {
    // this.getAllNotification();
  }
  private async getAllNotification() {
    this.notification = 0;
    let allNotification = await this.$store.dispatch({
      type: "news/GetAllNotification",
    });
    allNotification.forEach((element) => {
      if (!element.isRe) {
        this.notification++;
      }
      if (element.authorAvatar && element.authorAvatar.indexOf("http")) {
        element.authorAvatar = url + element.authorAvatar;
      }
    });
    this.listNotification = allNotification;
  }
  viewDetail(item: any) {
    if (item.commentId) {
      switch (item.entityName) {
        case InforEntity.New:
          this.$router.push({
            name: "NewsDetailEntity",
            params: {
              newsId: item.entityId,
              commentId: item.commentId ? item.commentId : "null",
            },
          });
          break;
        case InforEntity.Event:
          this.$router.push({
            name: "EventDetailEntity",
            params: {
              eventId: item.entityId,
              commentId: item.commentId ? item.commentId : "null",
            },
          });
          break;
        case InforEntity.Policy:
          this.$router.push({
            name: "policyDetailEntity",
            params: {
              policyId: item.entityId,
              commentId: item.commentId ? item.commentId : "null",
            },
          });
          break;
        case InforEntity.GuideLine:
          this.$router.push({
            name: "GuideDetailEntity",
            params: {
              guideId: item.entityId,
              commentId: item.commentId ? item.commentId : "null",
            },
          });
          break;
        case InforEntity.Blog:
          this.$router.push({
            name: "DetailListBlog",
            params: { blogId: item.entityId },
          });
          break;
        default:
          break;
      }
    } else {
      switch (item.entityName) {
        case InforEntity.New:
          this.$router.push({
            name: "NewsDetail",
            params: { newsId: item.entityId },
          });
          break;
        case InforEntity.Event:
          this.$router.push({
            name: "EventDetail",
            params: { eventId: item.entityId },
          });
          break;
        case InforEntity.Policy:
          this.$router.push({
            name: "policyDetail",
            params: { policyId: item.entityId },
          });
          break;
        case InforEntity.GuideLine:
          this.$router.push({
            name: "GuideDetail",
            params: { guideId: item.entityId },
          });
          break;
        case InforEntity.Blog:
          this.$router.push({
            name: "DetailListBlog",
            params: { blogId: item.entityId },
          });
          break;
        default:
          break;
      }
    }
    this.isNotification = false;
  }

  async clickNotification() {
    this.isNotification = !this.isNotification;
    await this.$store.dispatch({
      type: "news/clickNotification",
    });
    // this.getAllNotification();
  }
}
</script>
<style lang="scss" scoped>
@media only screen and (max-width: 1450px) {
  .menu-info {
    display: none;
  }
  .icon[data-v-aae802ba] {
    margin-top: -33px;
    font-size: 30px;
    float: right;
    margin-right: 79px;
  }
  .notification-list[data-v-aae802ba] {
    margin-top: -16px;
  }
  .notification .notification-number[data-v-aae802ba] {
    right: 152px;
    top: -62px;
  }
}
@media only screen and (max-width: 1450px) {
  .menu-info {
    display: none;
  }
  .icon[data-v-aae802ba] {
    margin-top: -33px;
    font-size: 30px;
    float: right;
    margin-right: 79px;
  }
  .notification-list[data-v-aae802ba] {
    margin-top: -16px;
  }
  .notification .notification-number[data-v-aae802ba] {
    right: 152px;
    top: -62px;
  }
}
@media only screen and (max-width: 1277px) and (min-width: 1115px) {
  .icon[data-v-aae802ba] {
    font-size: 30px;
    float: right;
    margin-right: 59px;
  }
}
@media only screen and (min-width: 1449px) {
  .nav-bar {
    display: none;
  }
}
@media only screen and (max-width: 415px) {
}
.icon {
  padding-top: 6px;
  font-size: 30px;
  float: right;
  margin-right: 70px;
}
.notification {
  float: right;
  position: relative;
  display: grid;
  .notification-number {
    font-size: 13px;
    width: 25px;
    border-radius: 50px;
    background-color: red;
    color: white;
    position: absolute;
    right: 131px;
    top: +8px;
  }
}
.notification-item:hover {
  color: indianred;
  font-weight: bold;
  cursor: pointer;
}

.notification-list {
  max-height: 480px;
  width: 380px;
  overflow: auto;
  background-color: white;
  float: right;
  .notification-item {
    display: flex;
    justify-content: flex-start;
    align-items: center;
    width: 100%;
    padding: 7px 5px;
    border-bottom: 1px solid #cccccc;

    img {
      width: 32px;
      border-radius: 50%;
      -moz-border-radius: 50%;
      -webkit-border-radius: 50%;
    }
    .messsage {
      padding-left: 5px;
      max-width: 400px;
      font-size: 14px;
      word-break: break-word;
    }
  }
  div:last-child {
    border-bottom: 0px solid #cccccc;
  }
}
</style>
