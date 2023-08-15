<template>
  <div class="event">
    <div v-if="isViewList">
      <!-- <BannerCarousel :data="allEvent" @openDetail="openEventDetail"/> -->
      <ListEvent @onCreateEvent="createEvent()" @viewEvent="onViewEvent" :allEvent="allEvent"></ListEvent>
    </div>
    <CreateEditEvent @backToList="onShowList" v-else-if="isViewCreateEdit"></CreateEditEvent>
    <EventDetail v-else :eventSelected="eventSelected"></EventDetail>
  </div>
</template>
<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import ListEvent from "./components/listEvent.vue";
import EventModule from "@/store/modules/event";
import EventDetail from "./components/eventDetail.vue";
import CreateEditEvent from "./components/createEditEvent.vue";
import url from '../../../lib/url'
import BannerCarousel from '../../../components/banner-carousel.vue';
@Component({
  components: { ListEvent, EventDetail, CreateEditEvent, BannerCarousel },
  props: {}
})
export default class Event extends Vue {
  private allEvent = [];
  private isViewList = true;
  private eventSelected: any = {};
  private isViewCreateEdit = false;
  async created() {
    let response = await this.$store.dispatch({
      type: "event/getAllEvents",
      data: {
        pageNumber: 1,
        pageSize: 4
      }
    });
    this.allEvent = response.items;
    this.allEvent.forEach(el => {
      el.image = url + el.image
      el.coverImage = url + el.coverImage
    })
  }

  onShowList() {
    this.isViewList = true;
  }

  onViewEvent(event) {
    this.eventSelected = event;
    let routeData = this.$router.resolve(`/information/events/${event.id}`);
    window.open(routeData.href, '_self');
  }

  createEvent() {
    this.$router.push({ name: 'CreateEvent' })
  }

  openEventDetail(id: number) {
    this.$router.push(`/information/events/${id}`)
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