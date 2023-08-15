<template>
  <div class="guide">
    <div v-if="isViewList">
      <!-- <BannerCarousel :data="allGuide" @openDetail="openEventDetail"/> -->
      <ListGuide @onCreateGuide="createGuide()" @viewGuide="onViewGuide" :allGuide="allGuide"></ListGuide>
    </div>
    <CreateEditGuide @backToList="onShowList" v-else-if="isViewCreateEdit"></CreateEditGuide>
    <GuideDetail v-else :guideSelected="guideSelected"></GuideDetail>
  </div>
</template>
<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import ListGuide from "./components/listGuide.vue";
import GuideModule from "@/store/modules/guide";
import GuideDetail from "./components/guideDetail.vue";
import CreateEditGuide from "./components/createEditGuide.vue";
import url from '../../../lib/url'
import BannerCarousel from '../../../components/banner-carousel.vue';
@Component({
  components: { ListGuide, GuideDetail, CreateEditGuide, BannerCarousel },
  props: {}
})
export default class Guide extends Vue {
  public allGuide = [];
  public isViewList = true;
  public guideSelected: any = {};
  public isViewCreateEdit = false;
  async created() {
    let response = await this.$store.dispatch({
      type: "guide/getAllGuides",
      data: {
        pageNumber: 1,
        pageSize: 10
      }
    });
    this.allGuide = response.items;
    this.allGuide.forEach(el => {
      el.image = url + el.image
      el.coverImage = url + el.coverImage
    })
  }

  onShowList() {
    this.isViewList = true;
  }

  onViewGuide(guide) {
    this.guideSelected = guide;
    let routeData = this.$router.resolve(`/information/guidelines/${guide.id}`);
    window.open(routeData.href, '_self');
  }

  createGuide() {
    this.$router.push({ name: 'CreateGuide' })
  }

  openEventDetail(id: number) {
    this.$router.push(`/information/guidelines/${id}`)
  }
}
</script>
<style lang="scss" scoped>
.item-car {
  padding: 30px;
  height: 250px;
  overflow: hidden;
  background: rgb(238, 174, 202);
  background: radial-gradient(
    circle,
    rgba(238, 174, 202, 1) 0%,
    rgba(240, 71, 53, 1) 100%
  );
}
.img-detail {
  padding-right: 50px;
  img {
    width: 100%;
  }
}
.new-title {
  font-size: 40px;
}
.description {
  font-size: 25px;
}
</style>