<template>
  <div class="list-new" style="width: 100%; margin:auto;">
    <div
      class="header d-flex flex-wrap flex-space-between"
      style=" background: white; padding-top:28px; padding-right:59px;"
    >
      <div class="d-flex col__item j-content" style="width: 33%; padding: 0 25px">
        <h3 class="col-label">{{ $t("news.allNews") }}</h3>
        <Button
          class="btn btn--create ml-2"
          v-if="SessionStore.state.user.canCreate"
          @click="onCreateNews"
          ghost
          >{{ $t("news.createNews") }}</Button
        >
      </div>
      <div class="d-flex d-block col__item">
        <Select
          v-model="searchTypeValue"
          style="width: 200px; margin-top: 1px;"
          :placeholder="$t('common.selectType')"
          class="ml-2 col__item m-header"
        >
          <Option v-for="item in typeList" :value="item.id" :key="item.id">{{
            item.typeName
          }}</Option>
        </Select>
        <Select
          v-if="!isGroupIdUse"
          v-model="searchStatusValue"
          style="width: 200px; margin-top: 1px;"
          :placeholder="$t('common.selectStatus')"
          class="ml-2 col__item m-header"
        >
          <Option
            v-for="item in statusList"
            :value="item.value"
            :key="item.value"
            >{{ item.label }}</Option
          >
        </Select>
        <Input
          v-model="searchValue"
          @on-search="fetchData"
          search
          class="ml-2 max-width col__item m-header"
          enter-button
          :placeholder="$t('common.enterSearchTitle')"
        />
      </div>
    </div>
    <Row 
      class="mt-4 mt-0 p-3"
      type="flex"
      justify="space-between"  style="margin-top: 0; padding:7px 40px;"
    >  
      <Col class="col m-content list_event"  v-for="(news,index) in listNews" :key="index" :xs="24" :sm="12" :md="8" :lg="8" >
        <NewsItem :data="news"  />
      </Col>
    </Row>
    <scroll-loader
      style="background-color: #f3f3f3"
      v-if="allNews.length < totalCount"
      :loader-method="getImagesInfo"
      :loader-enable="loadMore"
    >
    </scroll-loader>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import NewsView from "./NewsView.vue";
import url from "../../../../lib/url";
import SessionStore from "../../../../store/modules/session";
import NewsItem from "../../../../components/news-item.vue";
import { EStatus } from "../../../../types/information/EStatus";
import SessionStorageInfoPage from "../../../../types/session-storage/session-storage";
import IInfoPage from "../../../../types/session-storage/session-storage";
import ScrollLoader from "vue-scroll-loader";

Vue.use(ScrollLoader);
@Component({
  components: { NewsView, NewsItem },
  props: {},
})
export default class ListNews extends Vue {
  private loadMore: boolean = true;
  private pageSize: number = 8;
  private allNews: any = [];
  private listNews = [];
  private response: any = {};
  private currentPage = 1;
  private totalCount = 1;
  private searchValue: string = "";
  private searchTypeValue: string = "";
  private searchStatusValue: string = "";
  private SessionStore = SessionStore;
  private statusList = [];
  private typeList = [];
  private infoPage = new IInfoPage();
  private isGroupIdUse: boolean = false;

  onChangePage(page) {
    this.currentPage = page;
    this.fetchData();
  }

  viewNews(news) {
    this.infoPage.pageNumber = this.currentPage;
    SessionStorageInfoPage.save(this.infoPage);
    this.$emit("viewNews", news);
  }

  async isUser() {
    const response = await this.$store.dispatch({
      type: "news/init",
    });
    if (response.user.role === "Employee" || response.user.role === "Intern") {
      this.isGroupIdUse = true;
    }
  }

  async fetchData() {
    this.isUser();
    this.response = await this.$store.dispatch({
      type: "news/getAllNews",
      data: {
        title: this.searchValue,
        pageNumber: 1,
        pageSize: this.pageSize,
        status: this.searchStatusValue,
        id: this.searchTypeValue,
      },
    });

    this.allNews = this.response.items;
    this.totalCount = this.response.totalCount;
    this.listNews = [];
    this.listNews = this.response.items.map((el) => {
      el.isImage = el.image == null ? false : true;
      el.image = url + el.image;
      el.entityName = el.typeName;
      return el;
    });
    // this.allNews.forEach((el, i) => {
    //   if (i % 2 == 0) {
    //     el.image = url + el.image;
    //     el.entityName = el.typeName;
    //     if (this.allNews[i + 1]) {
    //       this.allNews[i + 1].image = url + this.allNews[i + 1].image
    //       this.listNews.push([el, this.allNews[i + 1]]);
    //     }
    //     else this.listNews.push([el]);
    //   }
    // });
    this.infoPage.entity = "New";
    this.infoPage.title = this.searchValue;
    this.infoPage.type = this.searchTypeValue;
    this.infoPage.status = this.searchStatusValue;
    this.infoPage.pageNumber = this.currentPage;
  }
  async created() {
    await this.$store
      .dispatch({
        type: "news/getEntityType",
        entity: "New",
      })
      .then((res) => (this.typeList = res.result));

    this.statusList = [
      {
        value: "",
        label: this.$t("common.all"),
      },
      {
        value: EStatus.Draft,
        label: this.$t("status.draft"),
      },
      {
        value: EStatus.WaitingForApproval,
        label: this.$t("status.waitingForApproval"),
      },
      {
        value: EStatus.Return,
        label: this.$t("status.return"),
      },
      {
        value: EStatus.Approved,
        label: this.$t("status.approved"),
      },
      {
        value: EStatus.Hidden,
        label: this.$t("status.hidden"),
      },
      {
        value: EStatus.Disabled,
        label: this.$t("status.disabled"),
      },
    ];
    if (SessionStorageInfoPage.hasValue("New")) {
      this.infoPage = SessionStorageInfoPage.getItem("New");
      this.searchValue = this.infoPage.title;
      this.searchTypeValue = this.infoPage.type;
      this.searchStatusValue = this.infoPage.status;
      this.currentPage = this.infoPage.pageNumber;
    }
    SessionStorageInfoPage.clear("GuildLine");
    SessionStorageInfoPage.clear("Policy");
    SessionStorageInfoPage.clear("Event");
    await this.fetchData();
  }

  onCreateNews() {
    this.infoPage.pageNumber = this.currentPage;
    SessionStorageInfoPage.save(this.infoPage);
    this.$emit("onCreateNews");
  }
  getImagesInfo() {
    this.pageSize = this.pageSize + 8;
    this.fetchData();
  }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
* {
  color: #333;
}
.list-new {
  background: #ffffff;
}
.col-label {
  text-align: left;
  font-size: 18px;
  margin-bottom: 10px;
  font-weight: bold;
  text-transform: uppercase;
  color: indianred;
  position: relative;
  padding-bottom: 8px;
  margin-bottom: -4px;
  margin-left: 31px;
  width: 100%;
}
.col-label::after {
  height: 1px;
  display: block;
  margin-top: 3px;
  width: 97%;
  background: #ccc;
  content: "";
}
::v-deep .single-post .ivu-card-body {
  max-height: 350px;
  min-height: 20vh;
}
// .box{
//    height: 12.6vh;

// }
.list_event {
  margin-bottom: 10px;
  /* height: 300px; */
}
.list_event .list-item {
  /* width: 330px ; */
  /* height: 270px !important; */
}
::v-deep input {
  height: 36px;
}
::v-deep .ivu-select-selection {
  height: 36px;
  padding-top: 2px;
}
::v-deep .ivu-card-body {
  height: auto;
  display: flex;
  flex-direction: column;
}
::v-deep .footer-left {
  display: none;
}

// @media only screen and (max-width: 768px) {
//   ::v-deep .list-item .box {
//     height: 100% !important;
    
//   }
//   @media only screen and (max-width: 992px ) and ( min-width: 767px) {
//   .box  {
//     height: 300px !important;
    
//   }
  
 
// }
// }
</style>
