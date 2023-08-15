<template>
 <div class="viewmyblog">
<Row>
    <Col class ="col col__item myblog" :xs="24" :sm="10" :md="10" :lg="10" >
      <Card >
         <Row>
           <Col span="24" v-if="isMyBlog">
           <Button class="btn btn--create ml-2"  @click="onCreateMyBlog">+ {{$t("blog.createBlog")}}</Button>
           <Button class="btn btn--create ml-2" style="margin-left:10px" @click="onEditInforBlog"> Sửa thông tin blog cá nhân</Button></Col>
        
           <Col span="8" align="left" style="margin-top: 100px;">
            <img style="border-radius: 50%" v-bind:src="inforMyblog.avatarUrl" alt="img" height="160" />
           </Col>
           <Col span="16" style="margin-top: 100px; border: 1px solid #CCCCCC;">
            <Col span="24" style="margin-top: 20px; padding-left: 5px;"><h3>{{inforMyblog.fullName}}</h3></Col>
            <Col span="24" style="margin: 20px 0px 30px 0px; padding-left: 5px;"><span><span style="color: tomato">{{$t("blog.nickname")}}</span> : {{inforMyblog.nickName}} </span></Col>
            <p style="color: #2E64FE; padding-left: 5px;">{{$t("blog.statistical")}}</p>
            <Col span="24" class="icon-infor-myblog">
              <Col span="6" align="center"  style="margin-top: 20px">
                <div class="infor-myblog">
                  <Icon class="icon-my-blog" type="ios-clipboard-outline" />
                  <p style="margin-top: 15px"> {{totalPost}} </p>
                </div>
              </Col>
              <Col span="6" align="center"  style="margin-top: 20px">
                <div class="infor-myblog">
                  <Icon class="icon-my-blog" type="md-thumbs-up" />
                  <p style="margin-top: 15px"> {{inforMyblog.totalReaction || 0 }} </p>
                </div>
              </Col>
              <Col span="6" align="center"  style="margin-top: 20px">
                <div class="infor-myblog">
                  <Icon class="icon-my-blog" type="md-chatboxes" />
                  <p style="margin-top: 15px"> {{inforMyblog.totalComment || 0  }} </p>
                </div>
              </Col>
              <Col span="6" align="center"  style="margin-top: 20px">
                <div class="infor-myblog">
                  <Icon class="icon-my-blog" type="md-eye" />
                  <p style="margin-top: 15px"> {{inforMyblog.totalView || 0  }} </p>
                </div>
              </Col>
              
            </Col>
            </Col>
          
         <template v-if="!isviewMore" > <Col span="24" align="center" class="item-description">
          <span><span style="color: tomato">{{$t("blog.description")}}</span> : {{inforMyblog.description}}</span></Col>
          <Col span="24" align="center"><a style="color: blue" @click="viewMore">Xem thêm...</a></Col></template>
          <template v-else>
           <Col span="24" align="center" style="margin-top: 20px;">
          <span ><span style="color: tomato">{{$t("blog.description")}}</span> : {{inforMyblog.description}}</span></Col>
          <Col span="24" align="center"><a style="color: blue" @click="hideMore">Ẩn bớt...</a></Col></template>
          </Row>
      </Card>
    </Col>
    <Col class ="col col__item listmyblog" :xs="24" :sm="14" :md="14" :lg="14">
      <Card v-if="allMyBlogs.length == 0" >
        Hiện tại, bạn chưa có bài viết nào <br/>
        Hãy tạo mới blog, để thêm bài viết của bạn nhé !</Card>
      <Card v-for="(item,index) in allMyBlogs"  :key="index" style="margin-top:10px;">
        <Row type="flex" justify="space-between">
       <Col span="24" class="list-item-right">
        <div class="user-item">
          <img class="img-item" v-bind:src="item.urlAvatar"  alt="img" height="60" />
          <div>
            <h4 class="title" @click ="onDetailBlog(item.postId)">
              {{item.tilte}}
            </h4>
            <span class="glyphicon glyphicon-user" style="margin-right:5px"></span><span style="margin-right: 10px;">{{item.fullName}}</span>
            <span>{{item.timeCreated}}</span>
            <span class="glyphicon glyphicon-thumbs-up" style="margin-left:10px; margin-right:2px;"></span><span>{{item.totalReaction || 0}}</span>
            <span class="glyphicon glyphicon-comment"   style="margin-left:10px; margin-right:2px;"></span><span>{{item.totalComment || 0 }}</span>
          </div>
        </div>
        <hr width="100%"/>
       
        <p class="item-detail" v-html="item.detail"></p>
        <span style="color: blue; padding-top:10px; float: right;"><a  @click ="onDetailBlog(item.postId)">Xem chi tiết... </a></span>
        <div style="height: 10px"></div>
      </Col>
      <hr width="100%"/>
      
      <Col span="24" style="text-align: right; margin-top:15px">
      
       <span v-if="item.reaction == null">{{ item.totalReaction }}<Icon @click="onLikeBlog(item.postId, item, index)" class="icono-thumbs-up" type="md-thumbs-up" /></span>
       <span v-else class="thumbs-up-on">{{ item.totalReaction }}<Icon @click="onLikeBlog(item.postId, item, index)" class="icono-thumbs-up" type="md-thumbs-up" /></span>
      </Col>
      </Row>
      </Card>
    </Col>
</Row>
    <scroll-loader v-if="allMyBlogs.length < totalPost" :loader-method="getImagesInfo" :loader-enable="loadMore">
    </scroll-loader>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Emit, Prop, Watch } from 'vue-property-decorator';
import BlogModule from "@/store/modules/blog";
import BlogUserModule from "@/store/modules/blog-of-user";
import moment from 'moment';
import ScrollLoader from 'vue-scroll-loader';
import url from '../../../../lib/url';
Vue.use(ScrollLoader)

@Component({
  components: {
  //  blog
  }
})
export default class ViewMyBlog extends Vue {
 private isViewMyblog = true;
 private loadMore: boolean =  true;
 private pageSize: number = 4;
 private allMyBlogs = [];
 private inforMyblog = {};
 private totalPost = 1;
 private totalCount = 1;
 private isviewMore = false;
 
 @Watch("$route")
  getEvent() {
    this.getImagesInfo()
  }

 private beforeMount() {
   this.getImagesInfo()
 }

 async fetchData() {
    let response = await this.$store.dispatch({
      type: "blog/getAllBlogFeed",
      data: {
        isInBlogFeed: 3,
        pageNumber: 1,
        pageSize: this.pageSize,
        userId: this.$route.params.userId ? this.$route.params.userId : this.$store.state.session.user.id,
      }
    });
   this.allMyBlogs = response.items.map(el => {
       el.detail = this.removePhotos(el.detail)
      return {
        ...el,
        timeCreated:  moment(el.timeCreated).format('DD-MM-YYYY'),
        urlAvatar: url + el.urlAvatar,
        detail: this.getHasshtag(el.detail)
       
     }
   });

   this.totalPost = response.totalCount;
   let response1 = await this.$store.dispatch({
      type: "bloguser/detailBlog",
      id: this.$route.params.userId ? this.$route.params.userId : this.$store.state.session.user.id,
    });
    response1.avatarUrl = url + response1.avatarUrl;
    this.inforMyblog = response1;
  //  this.inforMyblog.avatarUrl = url + response1.avatarUrl
  }

  removePhotos(data) {
    let detail = '';
    let before = data.slice(0, data.indexOf('<img')); // chuỗi trc hashtag
    let after = data.slice(data.indexOf('<img'),data.length) // chuỗi còn lại
    let final = after.slice(after.indexOf('<img'), after.indexOf('/>') +  2) // chuỗi hashtag
    let afterfinal = after.slice(final.length,after.length ) // chuỗi sau hashtag
    if( afterfinal.includes('<img')) { afterfinal = this.removePhotos(afterfinal)}
    detail = before + afterfinal ;
    return detail;
  }

  getHasshtag(data) {
    let detail = '';
    let before = data.slice(0, data.indexOf('#')); // chuỗi trc hashtag
    let after = data.slice(data.indexOf('#'),data.length) // chuỗi còn lại
    let final = after.slice(after.indexOf('#'), after.indexOf(' ')) // chuỗi hashtag
    let afterfinal = after.slice(final.length,after.length ) // chuỗi sau hashtag
    if( afterfinal.includes('#')) { afterfinal = this.getHasshtag(afterfinal)}
    let hashtag = final.link("#")
    detail = before + hashtag.bold() + afterfinal ;
    return detail;
  } 
  get isMyBlog(){
    if (this.$route.params.userId && this.$store.state.session.user && this.$route.params.userId !== this.$store.state.session.user.id) {
      return false
    }
    return true
  }
  
  viewMore(){
    this.isviewMore = true;
  }
  hideMore(){
    this.isviewMore = false;
  }
  onCreateMyBlog() {
     this.$emit("onCreateMyBlog");
  }

  onDetailBlog(id){
     this.$emit("onDetailBlog", id);
  }

  onEditInforBlog() {
    this.$emit("onEditInforBlog");
  }

   getImagesInfo() {
      this.pageSize = this.pageSize + 4
      this.fetchData();
  }
  private async onLikeBlog(id, item, index){
    let formData = {
      postId: id,
      reaction: 1
    };
    let response = await this.$store.dispatch({
      type: "blog/reactionPost",
      data: formData,
    });
    this.allMyBlogs[index].reaction = response.reaction;
     this.allMyBlogs[index].totalReaction = response.totalReact; 
  }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
.thumbs-up-on {
  color:blue;
}
.icono-thumbs-up {
  width: 20px;
}
.myblog {
  overflow: auto;
  position: fixed;
  max-height: 750px;
}
.listmyblog{
  float: right;
  margin-top: -10px;
}
@media only screen and (max-width: 768px) { 
   .myblog {
     overflow: auto;
     position:static;
     margin-bottom: 10px;  
}
//  .mt-dd-2[data-v-14b6294c] {
//    margin-top: 480px;
//  }
}
.img-item {
  width: 60px;
  height: 60px;
  margin-right: 15px;
  border-radius: 50%
}
.title{
  padding: 10px 0px;
  font-weight: bold;
  color: blue;
}
.user-item {
  padding-bottom: 10px;
  display:flex;
}
.item-detail {
  padding-top: 10px;
  overflow: hidden;
  cursor: pointer;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-box-orient: vertical;
  -webkit-line-clamp: 3;
  font-size: 13px;
  line-height: 2em;
  max-height: 82px;
}
.item-description {
  padding-top: 10px;
  overflow: hidden;
  cursor: pointer;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-box-orient: vertical;
  -webkit-line-clamp: 3;
  font-size: 14px;
  line-height: 2em;
  max-height: 82px;
}
.title:hover{
  color: deepskyblue;
 }
.icon-my-blog {
  font-size: 32px;
}
.infor-myblog {
  color: #2E64FE;
  margin: 2px;
}
.icon-infor-myblog {
  border-top: 2px solid #2E64FE ;
}
</style>