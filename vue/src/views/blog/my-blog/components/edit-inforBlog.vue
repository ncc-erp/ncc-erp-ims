<template>
  <div class="EditInforBlog">
    <h2>{{ $t("blog.updateInfor")}}</h2>
   
      <Row style="margin-bottom:20px">
          <Col span="1" offset="22" >
            <Button class="btn btn--save" @click="updateInforBlog" style="float: right;">{{$t("common.save")}}</Button>
          </Col>
          <Col span="1" >
            <Button class="btn btn--back"  @click="cancelModule" style="float: left;">{{$t("common.return")}}</Button>
          </Col>
       </Row>
        <Divider />
      <Row>
          <Col class="col" span="24">
            <Row class="row mb-2">
             <Col span="3" class="col col-label text-right col__item ">
                {{ $t("blog.avartarBlog")}}
             </Col>
             <Col span="15" class="col col__item">
               <div class="profile-wapper">
                            <div class="profile-avt" v-if="urlIMG">
                                <img :src="urlIMG" alt="" accept=".jpg, .png">
                            </div>
                            <div class="profile-avt" v-if="!urlIMG">
                                <img :src="img.buffer" alt="" >
                            </div>
                            <div class="profile-address">
                                <input type="button" value="Chọn ảnh" style="width: 25%;"
                                        onclick="document.getElementById('uploadImages').click();" />
                                <input type="file" @change="changeInmage($event)" id="uploadImages"  style="display: none">
                            </div>
                </div>
              <!------------->
           </Col>
         </Row>
        </Col>
        <!----tên user---->
        <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="3" class="col col-label text-right col__item title-left-dd">
            {{ $t("blog.nickname")}}
          </Col>
          <Col span="21" class="col col__item">
            <Input v-model="formInforBlog.nickName" />
            <span
              v-if="!formInforBlog.nickName && isClickSubmit"
              class="error-text"
            >{{$t("common.errorEmpty", { field: $t("blog.nickname")})}}</span>
          </Col>
        </Row>
      </Col>
      <!--Mô tả blog--->
      <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="3" class="col col-label text-right col__item title-left-dd">
            {{ $t("blog.description")}}
          </Col>
          <Col span="21" class="col col__item">
            <Input type="textarea" :rows="8" v-model="formInforBlog.description" />
            <span
              v-if="!formInforBlog.description && isClickSubmit"
              class="error-text"
            >{{$t("common.errorEmpty", { field: $t("blog.description")})}}</span>
          </Col>
         </Row>
        </Col>

    <!--ảnh nền--->
    <!-- <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="3" class="col col-label text-right col__item title-left-dd">
            {{ $t("blog.backgroundImage")}}
          </Col>
          <Col span="21" class="col col__item">
            <input class="input-file" type="file" @change="handleInputFile" accept="image/*" />
            <p v-if="editIMG">{{ editIMG }}</p>
          </Col>
        </Row>
      </Col> -->
    <!---Nhạc nền--->

      </Row>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Emit, Prop, Watch } from 'vue-property-decorator';
import { IFormUpdateInforBlog, IObjectFile} from '@/types/blog/createEditBlog';
import url from '../../../../lib/url';
import VueCroppie from 'vue-croppie';
import 'croppie/croppie.css';
Vue.use(VueCroppie)
Vue.config.productionTip = false
@Component({
  components: {
  }
})
export default class EditInforBlog extends Vue {
private urlIMG: string = '';
private isCrop: boolean = false;
private isCoverImage: boolean = false;
private isClickSubmit: boolean = false;
private formInforBlog = {} as IFormUpdateInforBlog;
private editIMG: string = '';
private cropped: any = null;
private  file: File = null;
private  img = {
    buffer: null,
    file: null,
    path: '',
  } as IObjectFile;
async beforeMount() {
     this.mapformCreateNew()
  }

private async mapformCreateNew() {
    let response = await this.$store.dispatch({
      type: "bloguser/detailBlog",
      id: this.$store.state.session.user.id,
    });
      this.formInforBlog = { ...this.formInforBlog }
      this.formInforBlog.nickName = response.nickName;
      this.formInforBlog.backgroundImages = null;
      this.formInforBlog.description = response.description;
      this.editIMG = url + response.backgroundImages;
      this.urlIMG = url + response.avatarUrl;
    }
    deleteImage(){}
  // async crop() {
    
  //     let options = {
  //       type: 'base64',
  //       size: { width: 1635, height: 300 },
  //       format: 'jpeg'
  //     };
  //     let ref: any = this.$refs.croppieRef
  //     await ref.result(options, (output) => {
  //       this.cropped = output
  //     })
  //     console.log(this.cropped)
  //       this.isCoverImage = true
  //       this.formInforBlog.avatar = this.cropped.replace("data:image/jpeg;base64,","");
  // }
  // croppie (e) {
  //   this.isCoverImage = false
  //   this.isCrop = true
  //     let files = e.target.files || e.dataTransfer.files;
  //     if (!files.length) return;
 
  //     let reader = new FileReader();
  //     reader.onload = e => {
  //       let ref: any = this.$refs.croppieRef
  //       ref.bind({
  //         url: e.target.result
  //       });
  //     };
 
  //   reader.readAsDataURL(files[0]);
  //   this.urlIMG = ''
  //   }
  changeInmage(event: any): void {
   this.urlIMG = ''
    const reader = new FileReader();
    this.img.file = event.target.files[0];
    reader.readAsDataURL(this.img.file);
    reader.onload = (_event) => {
      this.img.buffer = reader.result;
    };
  }

 result (output) {
      this.urlIMG = output
    }

 async handleInputFile(event: any) {
    this.formInforBlog.backgroundImages = event.target.files[0]
    if (this.formInforBlog.backgroundImages) {
      this.editIMG = ''
    }
  }

 private async updateInforBlog() {
     if(this.urlIMG) {
       this.formInforBlog.avatar = this.urlIMG;
     } else {
       this.formInforBlog.avatar = this.img.file;
     }
     this.isClickSubmit = true;
     this.formInforBlog.userId = this.$store.state.session.user.id;
     this.formInforBlog.backgroundImages = this.editIMG;
     let dataParam = this.formInforBlog;
      let formData = new FormData();
      for (let key in dataParam) {
            if (dataParam[key]) {
              formData.append(key, dataParam[key]);
            }
      }
      const reponse = await this.$store.dispatch({
        type: "bloguser/updateInfo",
        data: formData
      })
      this.isClickSubmit = false;
      this.$router.push('/blog/myblog');
}
  cancelModule(){ 
    this.$router.push('/blog/myblog')
  }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
.profile-avt img {
    max-width: 100%;
    max-height: 200px;
    object-fit: contain;
    margin-bottom: 10px;
}
.EditInforBlog{
    font-size: 16px;
}

</style>