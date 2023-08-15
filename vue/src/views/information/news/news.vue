<template>
  <div class="news">
    <div v-if="isViewList">
      <!-- <BannerCarousel :data="allNews" @openDetail="openEventDetail"/> -->
      <ListNews @onCreateNews="createNews()" @viewNews="onViewNews" :allNews="allNews"></ListNews>
    </div>
    <CreateEditNews @backToList="onShowList" v-else-if="isViewCreateEdit"></CreateEditNews>
    <NewsDetail v-else :newsSelected="newsSelected"></NewsDetail>
  </div>
</template>
<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import ListNews from "./components/listNews.vue";
import NewsModule from "@/store/modules/news";
import NewsDetail from './components/newsDetail.vue';
import CreateEditNews from './components/createEditNews.vue';
import url from '../../../lib/url';
import BannerCarousel from '../../../components/banner-carousel.vue';
@Component({
  components: { ListNews, NewsDetail, CreateEditNews, BannerCarousel },
  props: {}
})
export default class News extends Vue {
  private allNews = [];
  private isViewList = true
  private newsSelected: any = {}
  private isViewCreateEdit = false
  async created() {
    let response = await this.$store.dispatch({
      type: "news/getAllNews",
      data: {
        pageNumber: 1,
        pageSize: 4
      }
    });
    this.allNews = response.items;
    this.allNews.forEach(el => {
      el.image = url + el.image
      el.coverImage = url + el.coverImage
    })
  }

  onShowList() {
    this.isViewList = true
  }

  onViewNews(news) {
    this.newsSelected = news
    let routeData = this.$router.resolve(`/information/news/${news.id}`)
    window.open(routeData.href, '_self');
  }

  createNews() {
    this.$router.push({ name: 'CreateNews' })
  }

  openEventDetail(id: number) {
    this.$router.push(`/information/news/${id}`)
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
  font-size: 15px;
}
</style>