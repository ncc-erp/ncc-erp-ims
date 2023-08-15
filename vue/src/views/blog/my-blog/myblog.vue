<template>
  <div class="blog">
    <div v-if="isViewMyblog">
    <ViewMyBlog @onCreateMyBlog="onCreateMyBlog()" 
                @onDetailBlog="onDetailBlog"
                @onEditInforBlog="onEditInforBlog">
    </ViewMyBlog>
    </div>
    <CreateBlog  v-if="isViewCreateEdit"></CreateBlog>
    <DetailListBlog v-if="isVieưDetailBlog" ></DetailListBlog>
    <EditInforBlog v-if="isEditInforMyBlog"></EditInforBlog>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Emit, Prop, Watch } from 'vue-property-decorator'
import ViewMyBlog from './components/view-myblog.vue'
import CreateBlog from './components/createEdit-myblog.vue'
import BlogModule from "@/store/modules/blog";
import DetailListBlog from "../list-blog/components/detailListblog.vue"
import EditInforBlog from "../my-blog/components/edit-inforBlog.vue"
// import SessionStorageInfoPage from '../../types/session-storage/session-storage'
@Component({
  components: {
  ViewMyBlog,
  CreateBlog,
  DetailListBlog,
  EditInforBlog
  }
})
export default class MyBlog extends Vue {
    private isViewMyblog = true;
    private isViewCreateEdit = false;
    private isVieưDetailBlog = false;
    private isEditInforMyBlog = false;

   onCreateMyBlog() {
    this.isViewCreateEdit = true;
    this.$router.push({ name: 'CreateBlog' })
   }

   onDetailBlog(id: number) {
     this.isVieưDetailBlog = true;
     this.$router.push(`/blog/myblog/${id}`)
  }

  onEditInforBlog() {
   this.isEditInforMyBlog =true;
   this.$router.push({ name: 'EditInforBlog' })
  }
 
}
</script>
<style lang="scss" scoped>
</style>