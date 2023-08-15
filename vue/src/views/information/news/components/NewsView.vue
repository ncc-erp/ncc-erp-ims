<template>
  <Card class="news-view" style="width:100%">
    <Row type="flex" justify="space-between">
      <Col span="6" class="news-view-left">
        <div class="custom-img-box">
          <img :src="this.news.image" alt="img" height="130" />
        </div>
      </Col>
      <Col span="18" class="news-view-right">
        <h3 class="title">{{news.title}}</h3>
        <span>{{news.createDate | moment}}</span>
      </Col>
    </Row>
  </Card>
</template>
<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import moment from 'moment';
@Component({
  filters: {
    moment(value: any) {
      return moment(value).format('YYYY-MM-DD HH:mm')
    }
  }
})
export default class NewsView extends Vue {
  @Prop() private news: {};
}
</script>
<style lang="scss" scoped>
.news-view {
  max-height: 180px;
  .custom-img-box {
    width: 100%;
    height: 130px;
    img {
      object-position: center;
      object-fit: contain;
      width: 100%;
      height: 100%;
    }
  }
  .news-view-right {
    padding-left: 15px;
    span {
      font-size: 12px;
    }
  }
}
.news-view-detail {
  height: 150px;
  overflow: hidden;
}
</style>


