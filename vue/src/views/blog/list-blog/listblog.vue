<template>
  <div class="listblog">
    <div v-if="isViewMyblog">
    <ViewListBlog @onDetailBlog="onDetailBlog"></ViewListBlog>
    </div>
    <DetailListBlog  v-if="isViewDetailBlog"></DetailListBlog>
    <!-- <ListHashTag v-if="islistHashTag" @onDetailHashTag="onDetailHashTag"></ListHashTag> -->
    </div>
    
</template>
<script lang="ts">
import { Component, Vue, Inject, Emit, Prop, Watch } from 'vue-property-decorator'
import ViewListBlog from "./components/viewListblog.vue";
import BlogModule from "@/store/modules/blog";
import DetailListBlog from "./components/detailListblog.vue";
// import ListHashTag from "./components/listhashtag.vue";
// import SessionStorageInfoPage from '../../types/session-storage/session-storage'
@Component({
  components: {
      ViewListBlog, DetailListBlog
  }
})
export default class ListBlog extends Vue {
  private isViewMyblog = true;
  private isViewDetailBlog = false;
  // private islistHashTag = false;

onDetailBlog(id: number) {
  this.isViewDetailBlog = true;
  this.$router.push(`/blog/listblog/${id}`)
}
// onDetailHashTag(id: number) {
//   this.isViewDetailBlog = true;
//   this.$router.push(`/blog/listhashtag/${id}`)
// }
}
</script>
<style lang="scss" scoped>
</style>