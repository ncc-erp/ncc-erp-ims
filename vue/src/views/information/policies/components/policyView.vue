<template>
  <div class="policies-view">
    <Row type="flex" justify="space-between">
      <div class="policy-view">
      <Col span="12" class="new-detail">
        <h3 class="title">{{this.policies.title}}</h3>
      <div class="description" v-html="policies.description"> </div>
      </Col>
      <Col span="8" class="img-detail">
        <img :src="this.policies.image" alt="img" height="130" />
      </Col>
      </div>
    </Row>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
@Component({
  props: {}
})
export default class PolicyView extends Vue {
  @Prop() private policies: {};
 
}
</script>
<style lang="scss" scoped>
.policies-view {
  border: black 1px solid;
  border-radius: 5px;
  min-height: 150px;
  &:hover {
    .title {
      color: red;
    }
  }
}
.title {
  font-size: 16px;
  font-weight: 600;
}
.new-detail {
  overflow: hidden;
  padding: 10px 0px 0px 15px;
}
.img-detail {
  padding: 10px 10px 0px 0px;
}
.img-detail {
  img {
    width: 100%;
  }
}
.policy-view {
  height: 150px;
  overflow: hidden;
}
</style>


