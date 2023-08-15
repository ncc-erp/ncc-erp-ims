<template>
  <div class="createEdit-blog">
      <h2>{{!isEdit ? $t("blog.createBlog") : $t("blog.editBlog")}}</h2>
      <Divider />
      <Row style="margin-bottom:20px">
          <Col span="1" offset="22">
            <Button class="btn btn--save" @click="createBlog" style="float: right;">{{$t("common.save")}}</Button>
          </Col>
          <Col span="1">
            <Button class="btn btn--back"  @click="cancelModule" style="float: left;">{{$t("common.return")}}</Button>
          </Col>
      </Row>
      <Row>
          <!---title-->
           <Col class="col" span="24">
            <Row class="row mb-2">
             <Col span="4" class="col col-label text-right col__item title-left-dd">
                {{ $t("policy.title")}}
             </Col>
           <Col span="20" class="col col__item">
            <Input v-model="formCreateBlog.title" />
            <span
              v-if="!formCreateBlog.title && isClickSubmit"
              class="error-text"
            >{{$t("common.errorEmpty", { field: $t("policy.title")})}}</span>
          </Col>
          </Row>
        </Col>
        <!----content-->
        <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("policy.description")}}
          </Col>
          <Col span="20" class="col col__item">
            <Editor :value="formCreateBlog.detail" @onChange="handleDescriptionChange" id ="content" />
            <span
              v-if="!formCreateBlog.detail && isClickSubmit"
              class="error-text"
            >{{$t("common.errorEmpty", { field: $t("policy.description")})}}</span>
          </Col>
        </Row>
      </Col>
      <!--trạng thái-->
      <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("blog.status")}}
          </Col>
          <Col span="8" class="col col__item">
            <Select v-model="formCreateBlog.id">
              <Option v-for="item in status" :value="item.id" :key="item.id">{{ item.value }}</Option>
            </Select>
            <span
              v-if="!formCreateBlog.id && isClickSubmit"
              class="error-text"
            >{{$t("common.errorEmpty", { field: $t("blog.status")})}}</span>
          </Col>
        </Row>
      </Col>

      </Row>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Emit, Prop, Watch } from 'vue-property-decorator'
import { IFormCreateBlog } from '@/types/blog/createEditBlog'
import BlogModule from "@/store/modules/blog";
import editorComponent from '../../../tinymce/editorComponent.vue'
// import SessionStorageInfoPage from '../../types/session-storage/session-storage'
@Component({
  components: {
    'Editor': editorComponent
  }
})
export default class CreateBlog extends Vue {
private formCreateBlog = {} as IFormCreateBlog;
private isClickSubmit: boolean = false;
private isEdit: boolean = false
private status: any = [
    {
      id: 1,
      value: 'Public'
    },
    {
      id: 2,
      value: 'Private'
    },
  ];

handleDescriptionChange(value: string) {
    this.formCreateBlog.detail = value
}

private async createBlog() {
    this.isClickSubmit = true;
    if ( !this.isEdit) {
       let formData = {
         status: this.formCreateBlog.id,
         title: this.formCreateBlog.title,
         detail: this.formCreateBlog.detail
        }; 
        const reponse = await this.$store.dispatch({
        type: "blog/createBlogPost",
        dataBlog: formData
      });
      this.$router.push("/blog/myblog")
    } else {
      let formData = {
         id: Number(this.$route.params.myblogId),
         status: this.formCreateBlog.id,
         title: this.formCreateBlog.title,
         detail: this.formCreateBlog.detail
        }; 
      const reponse = await this.$store.dispatch({
        type: "blog/editBlogPost",
        dataBlog: formData
       });
       this.$router.push(`/blog/myblog/${this.$route.params.myblogId}`)
    }
  
  }
  async beforeMount() {
    this.mapformCreateNew();
  }

 private async mapformCreateNew() {
    if (this.$route.params.myblogId) {
      this.isEdit = true
      const response = await this.$store.dispatch({
        type: "blog/detailAPost",
        id: this.$route.params.myblogId
      })
      this.formCreateBlog = { ...this.formCreateBlog }
      this.formCreateBlog.title = response.tilte;
      this.formCreateBlog.detail = response.detail;
      this.formCreateBlog.id = response.status;
    }
  }
  cancelModule() {
     if (!this.isEdit) {
      this.$router.push('/blog/myblog')
     } else { this.$router.push(`/blog/myblog/${this.$route.params.myblogId}`) }
  }

}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
</style>