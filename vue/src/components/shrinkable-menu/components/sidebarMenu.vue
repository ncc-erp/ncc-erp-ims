<style lang="less">
@import "../styles/menu.less";
</style>

<template>
  <Menu
    mode="horizontal"
    ref="sideMenu"
    :active-name="activeName"
    :open-names="openNames"
    :theme="menuTheme"
    width="auto"
    class="menu-info"
    @on-select="changeMenu"
  >
    <template v-for="item in menuList">
      <MenuItem
        v-if="
          (item.children.length <= 1 && !item.hidden) ||
            (item.children.length == 4 && !item.hidden)
        "
        :name="item.name"
        :key="item.name"
        :to="item.path"
      >
        <span class="iconfontMenu" v-html="item.icon"> </span>
        <span class="route-name">{{ $t(`router.${item.name}`) }}</span>
      </MenuItem>
      <Submenu
        v-if="item.children.length > 1 && item.children.length !== 4"
        :name="item.name"
        :key="item.name"
      >
        <template slot="title">
          <span class="iconfontMenu" v-html="item.icon"></span>
          <span class="route-name">{{ $t(`router.${item.name}`) }}</span>
        </template>
        <template>
          <div class="card-submenu">
            <MenuItem
              v-for="child in item.children"
              v-if="!child.hidden"
              :name="child.name"
              :key="child.name"
              style="text-align: left; width: 100%;"
            >
              <span class="route-name-sub">{{
                $t(`router.${child.name}`)
              }}</span>
            </MenuItem>
          </div>
        </template>
      </Submenu>
    </template>
  </Menu>
</template>

<script lang="ts">
import { Component, Vue, Inject, Prop, Emit } from "vue-property-decorator";
import AbpBase from "../../../lib/abpbase";
@Component({})
export default class SidebarMenu extends AbpBase {
  name: string = "sidebarMenu";
  @Prop({ type: Array }) menuList: Array<any>;
  @Prop({ type: Number }) iconSize: number;
  menuTheme = "light";
  screenWidth = window.innerWidth + 40 + "px";
  @Prop({ type: Array }) openNames: Array<string>;
  itemTitle(item: any): string {
    return this.L(item.meta.title);
  }
  @Emit("on-change")
  changeMenu(active: string) {}
  updated() {
    this.$nextTick(() => {
      if (this.$refs.sideMenu) {
        (this.$refs.sideMenu as any).updateActiveName();
      }
    });
  }
  get activeName() {
    if (this.$route.name === "DashboardView") {
      return "Dashboard";
    } else {
      if (this.$route.name === "Calendar") {
        return "UserCalendar";
      } else {
        return this.$route.name;
      }
    }
  }
}
</script>
<style lang="scss" scoped>
::v-deep .ivu-menu-drop-list {
  list-style-type: none;
  margin: 0;
  padding: 0;
  overflow: hidden;
  // padding-left: 250px;
}
::v-deep .ivu-icon-ios-arrow-down:before {
  content: "";
}
::v-deep .ivu-select-dropdown {
  box-shadow: none;
  margin-left: -25px;
  top: 40px !important;
  margin-top: 20px;
}
::v-deep .ivu-menu-item {
  text-decoration: none;
}
.card-submenu {
  width: 180px;
}
::v-deep .ivu-menu-drop-list {
  padding: 15px 0;
  border: 1px solid #cacaca;
  border-radius: 5px;
}
::v-deep .ivu-select-dropdown {
  padding: 0px;
  border: 4px;
}
::v-deep .ivu-menu-item:hover {
  background: #f2f2f2;
  span {
    color: indianred;
  }
}
.route-name-sub {
  color: #000000;
  font-size: 15px;
  font-weight: 500;
}
[data-v-c9a65f5a] .ivu-menu-submenu .ivu-select-dropdown .ivu-menu-item {
  /* margin-top: 15px; */
  /* margin-left: 13px; */
  padding: 10px 16px 10px !important;
}
</style>
