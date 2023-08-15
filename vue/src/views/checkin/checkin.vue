<template>
  <div class="checkin">
     <iframe src="https://checkin.nccsoft.vn/ims/" style="height:750px;width:100%" allowfullscreen>
     </iframe>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Emit, Prop, Watch } from 'vue-property-decorator'
@Component({ 
  name: 'checkin',
  components: {
  }
})
export default class CheckIn extends Vue {
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
</style>

