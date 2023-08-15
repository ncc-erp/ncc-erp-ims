<template>
  <div class="list-new" style="width: 100%; margin:auto;">
    <div
      class="header d-flex flex-wrap flex-space-between"
      style=" background: white; padding-top:28px; padding-right:59px;"
    >
      <div class="d-flex col__item j-content" style="width: 33%; padding: 0 25px">
        <h3 class="col-label">{{ $t("guide.allGuide") }}</h3>
        <Button
          class="btn btn--create ml-2"
          v-if="SessionStore.state.user.canCreate"
          @click="onCreateGuide"
          ghost
          >{{ $t("guide.createPageTitle") }}</Button
        >
      </div>
      <div class="d-flex d-block col__item">
        <Select
          v-model="searchTypeValue"
          style="width: 200px; margin-top: 1px;"
          :placeholder="$t('common.selectType')"
          class="ml-2 col__item m-header"
        >
          <Option v-for="item in typeList" :value="item.id" :key="item.id">{{ item.typeName }}</Option>
        </Select>
        <Select
          v-if="!isGroupIdUse"
          v-model="searchStatusValue"
          style="width: 200px; margin-top: 1px;"
          :placeholder="$t('common.selectStatus')"
          class="ml-2 col__item m-header"
        >
          <Option v-for="item in statusList" :value="item.value" :key="item.value">{{ item.label }}</Option>
        </Select>
        <Input
          v-model="searchValue"
          @on-search="fetchData"
          search
          class="ml-2 max-width col__item m-header"
          style="height: 36px"
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
      <Col class="col m-content list_event"  v-for="(guide,index) in listGuide" :key="index" :xs="24" :sm="12" :md="8" :lg="8" >
        <NewsItem @click.native="viewGuide(guide)" :data="guide"  />
      </Col>
    </Row>
    <scroll-loader
      style="background-color: #f3f3f3"
      v-if="allGuide.length < totalCount"
      :loader-method="getMoreData"
      :loader-enable="loadMore"
    >
    </scroll-loader>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import url from "../../../../lib/url";
import GuideView from "./guideView.vue";
import SessionStore from "../../../../store/modules/session";
import NewsItem from "../../../../components/news-item.vue";
import { EPermission } from "../../../../types/information/EPermission";
import { EStatus } from "../../../../types/information/EStatus";
import SessionStorageInfoPage from "../../../../types/session-storage/session-storage";
import IInfoPage from "../../../../types/session-storage/session-storage";
import ScrollLoader from "vue-scroll-loader";

Vue.use(ScrollLoader);
@Component({
  components: { GuideView, NewsItem },
  props: {},
})
export default class ListGuide extends Vue {
  public loadMore: boolean = true;
  public pageSize: number = 8;
  public allGuide: any[] = [];
  public listGuide = [];
  public isGroupIdUse: boolean = false;
  public totalCount = 1;
  public searchValue: string = "";
  public searchTypeValue: string = "";
  public searchStatusValue: string = "";
  public SessionStore = SessionStore;
  public statusList = [];
  public typeList = [];
  private infoPage = new IInfoPage();
  private response: any = {};
  private currentPage = 1;
  private columnTable = [];

  onChangePage(page) {
    this.currentPage = page;
    this.fetchData();
  }

  viewGuide(guide) {
    this.infoPage.pageNumber = this.currentPage;
    SessionStorageInfoPage.save(this.infoPage);
    this.$emit("viewGuide", guide);
  }

  async isUser() {
    const response = await this.$store.dispatch({
      type: "news/init",
    });
    if (response.user.role === 'Employee'|| response.user.role === 'Intern') {
      this.isGroupIdUse = true
    }
  }

  async fetchData() {
    this.isUser()
    this.response = await this.$store.dispatch({
      type: "guide/getAllGuides",
      data: {
        title: this.searchValue,
        pageNumber: this.currentPage,
        pageSize: this.pageSize,
        status: this.searchStatusValue,
        id: this.searchTypeValue,
      },
    });
    this.allGuide = this.response.items;
    this.totalCount = this.response.totalCount;
    this.listGuide = [];

    this.listGuide = this.response.items.map((el) => {
      el.isImage = el.image == null ? false : true;
      el.image = url + el.image;
      el.entityName = el.typeName;
      return el;
    });

    this.infoPage.entity = 'GuildLine';
    this.infoPage.title = this.searchValue;
    this.infoPage.type = this.searchTypeValue;
    this.infoPage.status = this.searchStatusValue;
    this.infoPage.pageNumber = this.currentPage;
  }

  converStatus(value: number) {
    switch (value) {
      case EStatus.WaitingForApproval:
        return this.$t("status.waitingForApproval");
      case EStatus.Draft:
        return this.$t("status.draft");
      case EStatus.Return:
        return this.$t("status.return");
      case EStatus.Approved:
        return this.$t("status.approved");
      case EStatus.Hidden:
        return this.$t("status.hidden");
      case EStatus.Disabled:
        return this.$t("status.disabled");
    }
  }

  async created() {
    await this.$store
      .dispatch({
        type: "guide/getEntityType",
        entity: "GuildLine",
      })
      .then((res) => (this.typeList = res.result));
    this.columnTable = [
      {
        title: this.$t("policy.title"),
        slot: "title",
      },
      {
        title: this.$t("policy.type"),
        key: "typeName",
      },
      {
        title: this.$t("common.dateCreate"),
        slot: "date",
      },
      {
        title: this.$t("common.shortDesc"),
        key: "shortDescription",
      },
    ];
    if (
      SessionStore.state.user.groupId === EPermission.HR ||
      SessionStore.state.user.groupId === EPermission.CEO
    ) {
      this.columnTable.push({
        title: this.$t("common.selectStatus"),
        slot: "status",
      });
    }

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
    if(SessionStorageInfoPage.hasValue('GuildLine')){
      this.infoPage = SessionStorageInfoPage.getItem('GuildLine');
      this.searchValue = this.infoPage.title;
      this.searchTypeValue = this.infoPage.type;
      this.searchStatusValue = this.infoPage.status;
      this.currentPage = this.infoPage.pageNumber;
    }
    SessionStorageInfoPage.clear('Policy');
    SessionStorageInfoPage.clear('New');
    SessionStorageInfoPage.clear('Event');
    this.fetchData();
  }

  onCreateGuide() {
    this.infoPage.pageNumber = this.currentPage;
    SessionStorageInfoPage.save(this.infoPage);
    this.$emit("onCreateGuide");
  }

  getMoreData() {
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

.list_event {
  margin-bottom: 10px;
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

</style>
