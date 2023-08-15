<template>
  <Col
    class="col__item col-offset format"
    :xs="24"
    :sm="24"
    :md="24"
    :lg="24"
    span="24"
  >
    <Card class="box-format" v-if="listQuickNews" style="">
      <Row>
        <Col :xs="24" :sm="24" :md="24" :lg="24" span="24">
          <div style="display: flex;justify-content:space-between;">

            <b-icon class="iconBtnConvert" @click="convertKomuText()" icon="arrow-repeat" animation="spin-reverse" font-scale="4"></b-icon>
            <h3 class="title-left">TIN NHANH</h3>
            <Icon class="refresh_icon" type="ios-refresh" @click="refresh()"  size="20" />

          </div>
          <div v-for="(item, index) in listQuickNews" :key="index">
            <div style="margin-top: 8px;">
              <div
                :title="item.hover"
                class="description"
              >
                <a target="blank" style="list-style-type: disc">
                  <p
                    :id="item.id"
                    :class="{
                      showMore: item.isExpanded,
                      showLess: !item.isExpanded,
                    }"
                    class="text-des"
                  >
                   <span v-html="item.content" style="line-height: 20.5px;text-justify: inter-character;justify-content: center;text-align: justify;"></span>
                  </p>
                </a>
                <div style="display: flex; justify-content:space-between;">
                  <span
                  :id="'show' + item.id"
                  class="btn-showMore"
                  style="
                    
                  "
                  @click="isShowMore(item)"
                  v-if="
                    item.content.split('\n').length - 1 > 2 ||
                    item.content.length > 90
                  "
                  >{{ !item.isApprove ? "xem thêm" : " " }}
                </span>
                <span> </span>
                 <span class="createTime">{{ moment(item.creationTime) }}</span>
                </div>

               
              </div>
            </div>           
          </div>
          <span v-if="listQuickNews.length < totalCount"
             @click="getQuickNews()" style="
                    font-size: 14px;
                    color: #B24747;
                    font-weight: 600;
                    cursor: pointer;
                  "
              >Load more...</span>
        </Col>
      </Row>
    </Card>
  </Col>
</template>
<script lang="ts">
import { Component, Vue, Watch } from "vue-property-decorator";
import moment from "moment";
import QuickNewsDto from "@/store/entities/quickNews";
import {
  Hooper,
  Slide,
  Navigation as HooperNavigation,
  Pagination as HooperPagination,
} from "hooper";
import Hls from "hls.js";
import ScrollLoader from "vue-scroll-loader";
Vue.use(ScrollLoader);
@Component({
  components: {},
})
@Component({
  components: {
    Hooper,
    Slide,
    HooperNavigation,
    HooperPagination,
  },
})
export default class LayoutRight extends Vue {
  private listQuickNews = new Array<QuickNewsDto>();
  private totalCount = 0;
  private loadMore: boolean = true;
  private pageSize: number = 10;
  private check: boolean;

  convertKomuText(){
    this.getListConvertKomuText();
  }

  async getListConvertKomuText(){
    await this.$store
      .dispatch({
        type: "dashboard/changeFormatTextQuickNews",
        data: {
          pageNumber: 1,
          pageSize: this.pageSize,
        },
      })
      .then((res) => {
        this.listQuickNews = res.items;
        this.totalCount = res.totalCount;
      });
  }

  mounted() {
    this.getListQuickNews();
  }
  moment(value: any) {
    return moment(value).format("DD-MM-YYYY HH:mm");
  }
  refresh(){
    this.pageSize = 10;
    this.getListQuickNews();
  }

  async getListQuickNews() {
    await this.$store
      .dispatch({
        type: "dashboard/getAllQuickNews",
        data: {
          pageNumber: 1,
          pageSize: this.pageSize,
        },
      })
      .then((res) => {
        this.listQuickNews = res.items;
        this.totalCount = res.totalCount;
      });
  }
  
  checkDescriptionLength(des) {
    let count = 0;
    if (des.includes("\n")) {
      count++;
    }
    return count;
  }

  getQuickNews() {
    this.pageSize = this.pageSize + 10;

    this.getListQuickNews();
  }

  isShowMore(item) {
    item.isExpanded = !item.isExpanded;
    if (item.isExpanded) {
      document.getElementById(item.id).classList.add("showMore");
      document.getElementById(item.id).classList.remove("showLess");
      console.log(document.getElementById("show" + item.id));
      document.getElementById("show" + item.id).innerHTML = "rút gọn";
    } else {
      document.getElementById(item.id).classList.add("showLess");
      document.getElementById(item.id).classList.remove("showMore");
      document.getElementById("show" + item.id).innerHTML = "xem thêm";
    }
  }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
* {
  color: #333;
}
.btn-showMore{
  font-size: 12px;
  color: #B24747;
  font-weight: 600;
  cursor: pointer;
  font-family: "Roboto","Arial",sans-serif;
                    
}
.btn-showMore:hover {
  text-decoration: underline;
}
.box-format {
  box-shadow: 0 0 1.2rem rgb(0 0 0 / 15%);
  border-radius: 10px;
  border: none;
  padding: 20px 15px 10px 15px !important;
}

.iconBtnConvert{
  padding: 0px;
  margin: 0px;
  margin-top: 2px;
  height: 18px;
  width: 18px;
  cursor: pointer;
  color:black;
}

.refresh_icon{
  cursor: pointer;
  font-weight:600;
  color: #8a3434;
  margin-top:2px
}
.title-left {
  position: relative;
  font-family: Segoe UI;
  font-weight: bold;
  text-transform: uppercase;
  color: #8a3434;
  text-align: center;
  font-size: 20px;
  font-weight: bold;
  text-transform: uppercase;
  margin-left: 10px;
}

.showLess {
  font-size: 16px;
  max-height: 67px;
  line-height: 16.5px;
  overflow: hidden;
  display: block;
  display: -webkit-box;
  -webkit-box-orient: vertical;
  -webkit-line-clamp: 4;
  text-overflow: ellipsis;
}

.text-des {
  word-wrap: break-word;
  white-space: pre-line;
  line-height: 16.5px;
  font-size: 16px;
  font-weight: 400;
  font-family: sans-serif;
  color: #555;
}
.showMore {
  -webkit-line-clamp: unset !important;
}

input:focus ~ label {
  outline: -webkit-focus-ring-color auto 5px;
}

input:checked + .text-des {
  -webkit-line-clamp: unset;
}
.description{
  margin-top: -5px;
  border-bottom: 1px solid #8a3434;
}

input:checked ~ label {
  display: none;
}

.createTime {
  color: #888888 !important;
  font-family: "Roboto","Arial",sans-serif;
  font-size: 11px;
}
hr:last-of-type{
  margin-bottom: 8px;
}

</style>

