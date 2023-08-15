<template>
  <div class="list-new" style="background-color: #fff">
    <div
      class="header d-flex flex-wrap flex-space-between"
      style="padding-bottom: 0; padding-top: 28px; padding-right: 59px"
    >
      <div class="d-flex col__item j-content" style="width: 33%">
        <h3 class="col-label">{{ $t("event.allEvent") }}</h3>
        <Button
          class="btn btn--create ml-2"
          v-if="SessionStore.state.user.canCreate"
          @click="onCreateEvent"
          ghost
          >{{ $t("event.createPageTitle") }}</Button
        >
      </div>
      <div class="d-flex d-block col__item">
        <Select
          v-model="searchTypeValue"
          style="width: 200px"
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
          style="width: 200px"
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
    <Card style="box-shadow: none; border: none">
      <Row
        class="mt-4 mt-0"
        type="flex"
        justify="space-between"
        style="margin-top: 0"
      >
        <Col
          class="col col__mb col__item m-content list_event"
          v-for="event in listEvent"
          :key="event.id"
          :xs="24"
          :sm="12"
          :md="8"
          :lg="8"
        >
          <NewsItem @click.native="viewEvent(event)" :data="event" />
        </Col> </Row
    ></Card>
    <scroll-loader
      v-if="allEvent.length < totalCount"
      :loader-method="getImagesInfo"
      :loader-enable="loadMore"
    >
    </scroll-loader>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import EventView from "./eventView.vue";
import url from "../../../../lib/url";
import SessionStore from "../../../../store/modules/session";
import NewsItem from "../../../../components/news-item.vue";
import { EStatus } from "../../../../types/information/EStatus";
import SessionStorageInfoPage from "../../../../types/session-storage/session-storage";
import IInfoPage from "../../../../types/session-storage/session-storage";
import ScrollLoader from "vue-scroll-loader";

Vue.use(ScrollLoader);

@Component({
  components: { EventView, NewsItem },
  props: {},
})
export default class ListEvent extends Vue {
  private loadMore: boolean = true;
  private allEvent: any = [];
  private listEvent = [];
  private response: any = {};
  private currentPage = 1;
  private totalCount = 1;
  private searchValue = "";
  private searchTypeValue: string = "";
  private searchStatusValue: string = "";
  private SessionStore = SessionStore;
  private statusList = [];
  private typeList = [];
  private infoPage = new IInfoPage();
  private isGroupIdUse: boolean = false;
  private pageSize: number = 8;

  onChangePage(page) {
    this.currentPage = page;
    this.fetchData();
  }

  viewEvent(event) {
    this.infoPage.pageNumber = this.currentPage;
    SessionStorageInfoPage.save(this.infoPage);
    this.$emit("viewEvent", event);
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
      type: "event/getAllEvents",
      data: {
        title: this.searchValue,
        pageNumber: 1,
        pageSize: this.pageSize,
        status: this.searchStatusValue,
        id: this.searchTypeValue,
      },
    });
    this.allEvent = this.response.items;
    this.totalCount = this.response.totalCount;
    this.listEvent = [];
    // this.allEvent.forEach((el, i) => {
    //   if (i % 2 == 0) {
    //     el.image = url + el.image;
    //     if (this.allEvent[i + 1]) {
    //       this.allEvent[i + 1].image = url + this.allEvent[i + 1].image
    //       this.listEvent.push([el, this.allEvent[i + 1]]);
    //     }
    //     else this.listEvent.push([el]);
    //   }
    // });
    this.listEvent = this.response.items.map((el) => {
      el.isImage = el.image == null ? false : true;
      el.image = url + el.image;
      el.entityName = el.typeName;
      return el;
    });
    this.infoPage.entity = "Event";
    this.infoPage.title = this.searchValue;
    this.infoPage.type = this.searchTypeValue;
    this.infoPage.status = this.searchStatusValue;
    this.infoPage.pageNumber = this.currentPage;
  }
  async created() {
    await this.$store
      .dispatch({
        type: "event/getEntityType",
        entity: "Event",
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
    if (SessionStorageInfoPage.hasValue("Event")) {
      this.infoPage = SessionStorageInfoPage.getItem("Event");
      this.searchValue = this.infoPage.title;
      this.searchTypeValue = this.infoPage.type;
      this.searchStatusValue = this.infoPage.status;
      this.currentPage = this.infoPage.pageNumber;
    }
    SessionStorageInfoPage.clear("GuildLine");
    SessionStorageInfoPage.clear("Policy");
    SessionStorageInfoPage.clear("New");
    this.fetchData();
  }

  onCreateEvent() {
    this.infoPage.pageNumber = this.currentPage;
    SessionStorageInfoPage.save(this.infoPage);
    this.$emit("onCreateEvent");
  }
  getImagesInfo() {
    this.pageSize = this.pageSize + 8;
    this.fetchData();
  }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
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
::v-deep .ivu-input {
  height: 36px;
}
::v-deep .ivu-select-selection{
  height: 37px !important;
}

.list_event {
  margin-bottom: 10px;
}
</style>
