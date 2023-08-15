<template>
  <Carousel :autoplay-speed="5000">
    <CarouselItem v-for="item in data" :key="item.id">
      <div @click="openDetail(item)">
        <div class="img-background">
          <img :src="item.coverImage" alt="img" />
          <span class="item-background"></span>
          <Row type="flex" justify="space-between" class="item-card">
          <Col span="21" offset="3" class="item-detail">
            <h3 class="item-title title-mb">{{item.title}}</h3>
            <p>{{item.shortDescription}}</p>
          </Col>
          <Col span="8" class="img-detail">
          </Col>
          </Row>
        </div>
      </div>
    </CarouselItem>
  </Carousel>
</template>
<script lang="ts">
import { Component, Vue, Prop, Emit } from "vue-property-decorator";
import moment from 'moment';
@Component({
  filters: {}
})
export default class BannerCarousel extends Vue {
  @Prop() private data: any

  @Emit('openDetail') 
  openDetail(item: any) {
    return item.id
  }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
.item-card {
  z-index: 100;
  position: absolute;
  width: 100%;
  padding: 30px;
  height: 300px;
  overflow: hidden;
  .item-detail {
    color:white;
    height: fit-content;
  }
  .item-title {
    font-size: 30px;
  }
  .img-detail {
    padding-right: 50px;
    height: 100%;
    img {
      max-width: 100%;
      max-height: 100%;
      object-fit: cover;
      object-position: center;
    }
  }
}
.item-background {
  opacity: 0.2;
  z-index: 50;
  position: absolute;
  width: 100%;
  height: 100%;
  background-color: blue;
}
.img-background {
  position: relative;
  height: 300px;
  img {
      z-index: 1;
      position: absolute;
      width: 100%;
      height: 100%;
      object-fit: cover;
      object-position: center;
    }
}
</style>


