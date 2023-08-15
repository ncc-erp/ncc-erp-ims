<template>
  <div class="policies">
    <div>
      <!-- <BannerCarousel :data="allPolicies" @openDetail="openEventDetail"/> -->
      <ListPolicies @onCreatePolicy="onCreatePolicy" @viewPolicies="onViewPolicies" :allPolicies="allPolicies"
      ></ListPolicies>
    </div>
  </div>
</template>
<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import ListPolicies from "./components/listPolicies.vue";
import EditPolicy from "./components/editPolicies.vue"
import PoliciesDetail from "./components/policyDetail.vue"
import url from '../../../lib/url'
import BannerCarousel from '../../../components/banner-carousel.vue'
import ScrollLoader from 'vue-scroll-loader'

Vue.use(ScrollLoader)
@Component({
  components: { ListPolicies, EditPolicy, PoliciesDetail, BannerCarousel },
  props: {}
})
export default class Policies extends Vue {
  public allPolicies = [];
  public policiesSelected: any = {};
  public isViewCreateEdit = false

  async created() {
    let response = await this.$store.dispatch({
      type: "policy/getAllPolicies",
      data: {
        pageNumber: 1,
        pageSize: 10
      }
    });
    this.allPolicies = response.items;
    this.allPolicies.forEach(el => {
      el.image = url + el.image
      el.coverImage = url + el.coverImage
    })
  }
  
  onViewPolicies(policies) {
    this.policiesSelected = policies;
    let routeData = this.$router.resolve(`/information/policies/${policies.id}`);
    window.open(routeData.href, '_self');
  }

  onCreatePolicy() {
    this.isViewCreateEdit = true
  }

  openEventDetail(id: number) {
    this.$router.push(`/information/policies/${id}`)
  }
}
</script>
<style lang="scss" scoped>
</style>


