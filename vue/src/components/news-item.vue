<template>
 <div class="list-item" style="width:100%; height: 100%; position: relative; z-index: 1;">
  <div v-if = "this.data" class="single-post">
    <div class="box"  @click="viewDetail(data, $event)">
      <img v-if="this.data.isImage" class="img-item" :src="this.data.image" alt="img"/>
      <img v-else class="img-item" src="../images/image-gallery.png" alt="img" />
      <div class="tag">
          <img src="../assets/img/lable-news.png">
          <div class="tag-info">{{data.entityName}}</div>
     </div>    
    </div>
  <p>
     <div class="wrap-title" >
       <div class="title-post"  @click="viewDetail(data, $event)">
        {{data.title}}
      
      </div>
     </div>
    
     <span class="createDate">{{data.createDate | moment}}</span>
    <p class="description">{{data.shortDescription}}</p>
  
   
  </div>
  
</div>
</template>
<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import moment from "moment";
import { InforEntity } from "@/store/entities/inforEntity";
@Component({
  filters: {
    moment(value: any) {
      return moment(value).format("DD-MM-YYYY HH:mm");
    },
  },
})
export default class NewsITem extends Vue {
  @Prop() private data: {};
    viewDetail(item: any, e) {
    let route;
    switch (item.entityName) {
      case InforEntity.New:
        route = this.$router.push({
          name: "NewsDetail",
          params: { newsId: item.id },
        });
        break;
      case InforEntity.News:
        route = this.$router.push({
          name: "NewsDetail",
          params: { newsId: item.id },
        });
        break;
      case InforEntity.NCC8:
        route = this.$router.resolve({
          name: "NewsDetail",
          params: { newsId: item.id },
        });
        break;
      case InforEntity.NewStaff:
        route = this.$router.resolve({
          name: "NewsDetail",
          params: { newsId: item.id },
        });
        break;
      case InforEntity.Policy:
        route = this.$router.resolve({
          name: "policyDetail",
          params: { policyId: item.id },
        });
        break;
      case InforEntity.GuideLine:
        route = this.$router.resolve({
          name: "GuideDetail",
          params: { guideId: item.id },
        });
        break;
      case InforEntity.Event:
        route = this.$router.resolve({
          name: "EventDetail",
          params: { eventId: item.id },
        });
        // this.$router.push(`/information/events/${item.id}`);
        break;
      default:
        //   route = this.$router.resolve({
        //     name: "EventDetail",
        //     params: { eventId: item.id },
        //   });
        break;
    }
    if (e.ctrlKey) {
      window.open(route.href, "_blank");
    } else {
      window.open(route.href, "_self");
    }
    
  }
  
}
</script>
<style lang="scss" scoped>
.list-item {
  cursor: pointer;
  // .custom-img-box {
  //   width: 100%;
  //   height: 130px;
  img {
    object-position: center;
    object-fit: contain;
    width: 100%;
    height: 100%;
  }

  .box {
    background-color: black;
    width: 175px;
    height: 122px;
    float: left;
    margin-right: 10px;
    margin-bottom: 5px;
    position: relative;
    overflow: hidden;
    .img-item {
      max-width: 100%;
      max-height: 125px;
      position: absolute;
      top: 0;
      left: 0;
      //  transform: translate(-50%, -50%);
    }
    .img-item:hover {
      transform: scale(1.2);
      transition: transform 0.5s ease;
    }
  }

  .tag {
    max-width: 50px;
    position: absolute;
    // right: -5px;
    // top: -5px;
  }

  .tag-info {
    position: absolute;
    top: 40%;
    left: 50%;
    -webkit-transform: translate(-50%, -50%);
    transform: translate(-50%, -50%);
    color: #fff;
    font-size: 10px;
    text-align: center;
    width: 100%;
    overflow: hidden;
  }
  .title-post {
    text-transform: capitalize;
    margin-bottom: 5px;
    color: #333;
    font-size: 16px;
    font-weight: 700;
    line-height: 22px;
    position: relative;
  }
  .title-post:hover {
    text-decoration: none;
    color: #087cce;
  }
  
  .createDate {
    color: #777;
    font-size: 14px;
    display: block;
    margin-bottom: 10px;
  }
  .description,
  .title-post {
    overflow: hidden;
    text-overflow: ellipsis;
    display: -webkit-box;
    -webkit-line-clamp: 2; /* number of lines to show */
    -webkit-box-orient: vertical;
  }
  .description {
    font-size: 15px;
  }
  .list-item-right {
    padding-left: 15px;
    span {
      font-size: 12px;
      font-style: italic;
      margin-top: 20px;
    }
    h3 {
      display: flex;
      align-items: center;
      .type-tag {
        color: red;
        display: inline-block;
        border: 1px solid red;
        border-radius: 10px;
        padding: 0 5px;
        font-size: 10px;
        margin-left: 10px;
      }
    }
    p {
      height: 85px;
      overflow: hidden;
      text-overflow: ellipsis;
    }
  }
  
}
.list-item-detail {
  height: 150px;
  overflow: hidden;
}

// single-post
@media only screen and (max-width: 415px) {
  .single-post {
    display: flex;
    flex-direction: column;
    margin-bottom: 16px;
  }
  .box {
    width: 100% !important;
    height: 200px !important;
    margin-right: 0 !important;
  }
  .img-item {
    max-height: 200px !important;
  }
  .title-post {
    font-size: 20px !important;
    order: -1;
  }
  .description {
    text-align: justify;
    -webkit-line-clamp: 3 !important;
    font-size: 17px !important;
  }
  .tooltiptext {
    display: none;
  }
}
</style>


