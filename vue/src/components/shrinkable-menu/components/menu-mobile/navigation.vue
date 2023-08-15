<template>
  <div id="app" class="nav-bar">
    <nav class="main-nav">
      <Burger @clicked-showNav="clickShowNav" :isBurgerActive="isShowNav" />
    </nav>

    <Sidebar :isPanelOpen="isShowNav" @PanelOpen="clickPanel">
      <!-- <div @click="clickShowNav(false)" class="d-close">
        <Icon type="md-close" size="24" />
      </div> -->
      <Menu @on-select="changeMenu">
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
            <Icon type="md-document" />
            {{ $t(`router.${item.name}`) }}
          </MenuItem>
          <Submenu
            v-if="item.children.length > 1 && item.children.length !== 4"
            :name="item.name"
            :key="item.name"
          >
            <template slot="title">
              <Icon type="ios-analytics" />
              {{ $t(`router.${item.name}`) }}
            </template>
            <MenuItem
              :name="child.name"
              v-for="child in item.children"
              v-if="!child.hidden"
              :key="child.name"
            >
              <span>{{ $t(`router.${child.name}`) }}</span>
            </MenuItem>
          </Submenu>
        </template>
      </Menu>
    </Sidebar>
  </div>
</template>

<script lang="ts">
import Burger from "./burger.vue";
import Sidebar from "./sideBar.vue";
import { Component, Vue, Inject, Prop, Emit } from "vue-property-decorator";
@Component({
  components: { Burger, Sidebar },
})
export default class Navigation extends Vue {
  name: string = "shrinkableMenu";
  isShowNav = false;
  isPannel: Boolean;
  screenWidth = window.innerWidth + 40 + "px";
  @Prop({ type: Array }) menuList: Array<any>;

  clickPanel(param) {
    this.isShowNav = !this.isShowNav;
  }

  @Emit("on-change")
  changeMenu(active: string) {
    this.isShowNav = !this.isShowNav;
  }

  clickShowNav(param) {
    this.isShowNav = param;
  }
}
</script>
<style lang="scss" scoped>
.main-nav {
  display: flex;
  justify-content: space-between;
  padding: 0.5rem 0.8rem;
  float: left;
}

@media only screen and (min-width: 426px) {
  .d-close {
    display: none;
  }
}

ul.sidebar-panel-nav {
  list-style-type: none;
}

.ivu-menu-light {
  background-color: #f0f2f5 !important;
  width: 300px !important;
  top: 5%;
}

ul.sidebar-panel-nav > li > a {
  color: #f0f2f5;
  text-decoration: none;
  font-size: 1.5rem;
  display: block;
  padding-bottom: 0.5em;
}

.d-close {
  float: right;
  margin-right: 7px;
}
</style>
