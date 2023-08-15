<style lang="less">
@import "./main.less";
</style>
<template>
  <div class="main">
    <div
      class="main-header-con"
      style="display: inline-flex; align-items: center"
    >
      <div class="logo-header">
        <a>
          <img height="50" src="../images/IMS.svg" />
          <span class="logo" @click="returnDashboard()">IMS</span>
        </a>
      </div>
      <div class="search-header col-2 ">
        <!-- <input type="text" placeholder="Tìm kiếm" v-model="searchQuery" /> -->
        <div style="display: flex; position: relative">
          <input
            type="search"
            data-toggle="tooltip"
            data-placement="top"
            title="Hàng trưng bày!"
            @keydown.enter.exact.prevent="searchPost()"
            @input="hideClose()"
            placeholder="Tìm kiếm"
            v-model="searchQuery"
          />
          <span style=" position:absolute; right:17px">
            <button
              style=" margin-left: 10px;  background:none; border: none; margin-top: 12px;"
              @click="clear()"
              v-if="isClose == false"
            >
              <i class="fas fa-times fa-lg"></i>
            </button>
          </span>
        </div>
        <!--    v-if="$route.fullPath == '/dashboard' && isClose == false" -->
        <!-- v-if="$route.fullPath == '/dashboard'" -->
      </div>
      <div class="menu-header">
        <shrinkable-menu
          :shrink="shrink"
          @on-change="handleSubmenuChange"
          :theme="menuTheme"
          :before-push="beforePush"
          :open-names="openedSubmenuArr"
          :menu-list="menuList"
        >
        </shrinkable-menu>
      </div>
      <div class="main-header">
        <div class="header-avator-con">
          <!-- <full-screen v-model="isFullScreen" @on-change="fullscreenChange"></full-screen> -->
          <!-- <lock-screen></lock-screen>  
                    <notice></notice>   
                    <language-list></language-list>          -->
          <div class="user-dropdown-menu-con">
            <Row
              type="flex"
              justify="end"
              align="middle"
              class="user-dropdown-innercon"
            >
              <Dropdown
                transfer
                trigger="click"
                @on-click="handleClickUserDropdown"
              >
                <!-- <a href="javascript:void(0)">
                  <span class="main-user-name"
                    >{{ userName }} {{ surname }}</span
                  >
                  <Icon type="arrow-down-b"></Icon>
                </a>
                <p class="main-email">{{ emailAddress }}</p> -->
                <DropdownMenu slot="list">
                  <!-- <DropdownItem name="ownSpace">{{L('UserProfile')}}</DropdownItem> -->
                  <!-- <DropdownItem name="loginout" divided> -->
                  <DropdownItem name="loginout">
                    {{ L("Logout") }}</DropdownItem
                  >
                </DropdownMenu>
                <a class="avatar" style="background: #619fe7; margin-left: 10px"
                  ><img :src="Avartar"
                /></a>
              </Dropdown>
            </Row>
          </div>
        </div>
      </div>
      <!-- <div class="tags-con">
                <tags-page-opened :pageTagsList="pageTagsList"></tags-page-opened>
            </div> -->
    </div>
    <div class="single-page-con" :style="{ left: 0 }">
      <div class="single-page">
        <!-- <keep-alive :include="cachePage">
          <router-view></router-view>
        </keep-alive> -->
        <router-view></router-view>
      </div>
      <copyfooter :copyright="L('CopyRight')"></copyfooter>
    </div>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import shrinkableMenu from "../components/shrinkable-menu/shrinkable-menu.vue";
import tagsPageOpened from "../components/tags-page-opened.vue";
import breadcrumbNav from "../components/breadcrumb-nav.vue";
import fullScreen from "../components/fullscreen.vue";
import { EventBus } from "../main";
import lockScreen from "../components/lockscreen/lockscreen.vue";
import notice from "../components/notices/notice.vue";
import util from "../lib/util";
import copyfooter from "../components/Footer.vue";
import LanguageList from "../components/language-list.vue";
import AbpBase from "../lib/abpbase";
import url from "../lib/url";
@Component({
  components: {
    shrinkableMenu,
    tagsPageOpened,
    breadcrumbNav,
    fullScreen,
    lockScreen,
    notice,
    copyfooter,
    LanguageList,
  },
})
export default class Main extends AbpBase {
  shrink: boolean = false;
  searchQuery: string = "";
  isClose: boolean = true;
  get userName() {
    return this.$store.state.session.user
      ? this.$store.state.session.user.name
      : "";
  }
  get surname() {
    return this.$store.state.session.user
      ? this.$store.state.session.user.surname
      : "";
  }
  get emailAddress() {
    return this.$store.state.session.user
      ? this.$store.state.session.user.emailAddress
      : "";
  }
  url: string = url;
  get Avartar() {
    let avartar;
    if (this.$store.state.session.user) {
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
  isFullScreen: boolean = false;
  messageCount: string = "0";
  get openedSubmenuArr() {
    return this.$store.state.app.openedSubmenuArr;
  }
  get menuList() {
    return this.$store.state.app.menuList;
  }
  get pageTagsList() {
    return this.$store.state.app.pageOpenedList as Array<any>;
  }
  get currentPath() {
    return this.$store.state.app.currentPath;
  }
  get lang() {
    //
    return this.$store.state.app.lang;
  }
  get avatorPath() {
    return localStorage.avatorImgPath;
  }
  get cachePage() {
    return this.$store.state.app.cachePage;
  }
  get menuTheme() {
    return this.$store.state.app.menuTheme;
  }
  get mesCount() {
    return this.$store.state.app.messageCount;
  }
  init() {
    let pathArr = util.setCurrentPath(this, this.$route.name as string);
    this.$store.commit("app/updateMenulist");
    if (pathArr.length >= 2) {
      this.$store.commit("app/addOpenSubmenu", pathArr[1].name);
    }
    let messageCount = 3;
    this.messageCount = messageCount.toString();
    this.checkTag(this.$route.name);
  }
  toggleClick() {
    this.shrink = !this.shrink;
  }

  hideClose() {
    if (this.searchQuery == "") {
      this.isClose = true;
    } else {
      this.isClose = false;
    }
  }
  clear() {
    this.searchQuery = "";
    this.isClose = true;
    this.searchPost();
  }
  searchPost() {
    EventBus.$emit("inputData", this.searchQuery);
  }
  handleClickUserDropdown(name: string) {
    if (name === "ownSpace") {
      util.openNewPage(this, "ownspace_index", null, null);
      this.$router.push({
        name: "ownspace_index",
      });
    } else if (name === "loginout") {
      this.$store.commit("app/logout", this);
      util.abp.auth.clearToken();
      location.reload();
    }
  }
  checkTag(name?: string) {
    let openpageHasTag = this.pageTagsList.some((item: any) => {
      if (item.name === name) {
        return true;
      } else {
        return false;
      }
    });
    if (!openpageHasTag) {
      util.openNewPage(
        this,
        name as string,
        this.$route.params || {},
        this.$route.query || {}
      );
    }
  }
  handleSubmenuChange(val: string) {}
  beforePush(name: string) {
    if (name === "accesstest_index") {
      return false;
    } else {
      return true;
    }
  }
  fullscreenChange(isFullScreen: boolean) {}
  @Watch("$route")
  routeChange(to: any) {
    this.$store.commit("app/setCurrentPageName", to.name);
    let pathArr = util.setCurrentPath(this, to.name);
    if (pathArr.length > 2) {
      this.$store.commit("app/addOpenSubmenu", pathArr[1].name);
    }
    this.checkTag(to.name);
    localStorage.currentPageName = to.name;
  }
  @Watch("lang")
  langChange() {
    util.setCurrentPath(this, this.$route.name as string);
  }
  mounted() {
    this.init();
  }
  created() {
    this.$store.commit("app/setOpenedList");
  }
  returnDashboard() {
    this.$router.push(`/dashboard/`);
  }
}
</script>
<style lang="scss" scoped>
::v-deep .main-header-con {
  position: relative;
  padding-left: unset;
  height: 71px;
  background: #ffffff 0% 0% no-repeat padding-box;
  box-shadow: 0px 1px 3px #a4a4a4;
  /* box-shadow: 0 1px 1px 1px #cd5c5c; */
  opacity: 1;
}
::v-deep .main-header {
  .header-avator-con {
    width: 100px;
  }
  height: 72px;
}
::v-deep .single-page-con {
  height: calc(100% - 71px);
  top: 74px;
  background: #f9f9f9;
}
::v-deep .ivu-menu-horizontal {
  width: fit-content;
  margin-left: 5%;
}
::v-deep .ivu-menu-light.ivu-menu-horizontal .ivu-menu-item,
::v-deep .ivu-menu-light.ivu-menu-horizontal .ivu-menu-submenu {
  border-bottom: unset;
  white-space: nowrap;
  font-weight: 600;
  /* color: #000000; */
  line-height: 72px;
  display: flex !important;
  align-items: center !important;
  padding: 0;
  margin-right: 30px;
}
::v-deep .ivu-menu-light.ivu-menu-horizontal .ivu-menu-item,
::v-deep
  .ivu-menu-light.ivu-menu-horizontal
  .ivu-menu-submenu
  .ivu-menu-submenu-title {
  border-bottom: unset;
  white-space: nowrap;
  font-weight: 500;
  /* color: #000000; */
  line-height: 72px;
  display: flex !important;
  align-items: center !important;
}
::v-deep .ivu-menu-light.ivu-menu-horizontal .ivu-menu-item {
  color: #000000;
}
::v-deep .ivu-menu-light.ivu-menu-horizontal .ivu-menu-submenu {
  color: #000000;
}
::v-deep .ivu-card-body {
  /* background: #f3f3f3; */
}

::v-deep .ivu-menu-light.ivu-menu-horizontal .ivu-menu-item-active,
::v-deep .ivu-menu-submenu-active,
::v-deep .ivu-menu-light.ivu-menu-horizontal .ivu-menu-item:hover,
::v-deep .ivu-menu-light.ivu-menu-horizontal .ivu-menu-submenu:hover,
::v-deep
  .ivu-menu-horizontal
  .ivu-menu-submenu
  .ivu-select-dropdown
  .ivu-menu-item-selected {
  //background-color : indianred !important;
  font-weight: 700;
  color: indianred !important;
  border-bottom: unset;
}
::v-deep .menu-info .ivu-menu-item {
  background: unset;
}
::v-deep .ivu-menu-horizontal.ivu-menu-light:after {
  background-color: #fff;
}
::v-deep
  .ivu-menu-horizontal
  .ivu-menu-submenu
  .ivu-select-dropdown
  .ivu-menu-item {
  float: left;
  clear: none;
  /* margin-left: 20px; */
}
.menu-header {
  width: 100%;
}
input {
  outline: none;
}
input::-webkit-search-decoration,
input::-webkit-search-cancel-button {
  display: none;
}

input[type="search"] {
  -webkit-appearance: textfield;
  -webkit-box-sizing: content-box;
  font-family: inherit;
  font-size: 100%;
  background: #ededed url(../images/search.svg) no-repeat 15px center;
  border: none;
  padding: 0px 0px 0px 35px;
  width: 160px;
  height: 41px;
  -webkit-border-radius: 10em;
  -moz-border-radius: 10em;
  border-radius: 10em;
  margin-left: 16px;
  -webkit-transition: all 0.5s;
  -moz-transition: all 0.5s;
  transition: all 0.5s;
}
input[type="search"]::placeholder {
  color: #707070;
  font-size: 14px;
}
// input[type=search]:focus {
// 	width: 200px;
// 	background-color: #fff;
// 	border-color: #cc9766;
// 	-webkit-box-shadow: 0 0 5px rgb(246, 109, 109);
// 	-moz-box-shadow: 0 0 5px rgba(246, 109, 109);
// 	box-shadow: 0 0 5px rgba(246, 109, 109);
// }

input:-moz-placeholder {
  color: #999;
}
input::-webkit-input-placeholder {
  color: #999;
}
.logo-header {
  width: 175px;
  padding-left: 25px;
}
.logo-header a {
  display: flex;
  align-items: flex-end;
  span {
    margin-left: 5px;
  }
  img {
    margin-bottom: 10px;
  }
}
span.logo {
  line-height: 80px;
  font-size: 26px;
  font-weight: bold;
  color: #8a3434;
}
.user-dropdown-menu-con {
  width: 246px !important;
}
.main-user-name {
  width: 132px !important;
}
//single-page
.menu-header::v-deep .burger-button[data-v-2f255572] {
  background: transparent;
}

@media only screen and (max-width: 415px) {
  .single-page {
    margin: 0 !important;
  }
  .single-page-con {
    overflow-x: hidden;
  }
  .menu-header {
    order: -1;
    width: 30%;
  }
  //  .menu-header::v-deep .burger-button[data-v-2f255572] {
  //    right: 0;
  //  }
  .menu-header::v-deep #app {
    display: flex;
    justify-content: flex-start;
    padding-left: 14px;
  }
  .menu-header::v-deep #app .main-nav {
    padding: 0.5rem 0;
  }
  .main-header {
    width: 30% !important;
    border-bottom: 1px solid #d4d4d4;
    box-shadow: none;
  }
  .logo-header {
    width: 40%;
    text-align: center;
  }
  .logo-header a {
    padding-left: 0;
  }
  .main-user-name {
    width: 66px !important;
  }
  .user-dropdown-innercon p {
    text-overflow: ellipsis;
    text-align: right;
    width: 66px;
    overflow: hidden;
  }
}
@media only screen and (max-width: 1550px) {
  ::v-deep .ivu-menu-horizontal {
    margin-left: 1.5%;
    .iconfontMenu {
      svg {
        width: 25px;
        height: 25px;
        padding-top: 5px;
      }
      .route-name {
        font-size: 17px;
      }
    }
  }
}
@media only screen and (min-width: 1550px) {
  ::v-deep .ivu-menu-horizontal {
    margin: 0 auto;
    padding-right: 5%;
  }
}
@media only screen and (min-width: 1550px) {
  ::v-deep .ivu-menu-horizontal {
    margin: 0 auto;
    padding-right: 5%;
  }
}
@media only screen and (min-width: 1550px) {
  ::v-deep .ivu-menu-horizontal {
  }
}
@media only screen and (max-width: 650px) {
  .search-header {
    display: none;
  }
}
@media only screen and (max-width: 1450px) {
  .logo-header {
    width: 240px;
    padding-left: 70px;
  }
}
@media only screen and (max-width: 415px) {
  .logo-header {
    width: 240px;
    padding-left: 30px;
  }
  .main-header .header-avator-con .user-dropdown-menu-con .main-user-name {
    display: none;
  }
  .main-email {
    display: none;
  }
}
</style>
