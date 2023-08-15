<template>
  <div class="list-new" style="background-color: #fff">
    <div
      class="header d-flex flex-wrap flex-space-between"
      style="padding-bottom: 0; padding-top: 28px; padding-right: 59px"
    >
      <div class="d-flex col__item j-content" style="width: 33%">
        <h3 class="col-label">{{ $t("policy.allPolicies") }}</h3>
        <Button
          class="btn btn--create ml-2"
          v-if="SessionStore.state.user.canCreate"
          @click="redirectToEditPolicy"
          ghost
          >{{ $t("policy.createPageTitle") }}</Button
        >
      </div>
      <div class="d-flex d-block col__item">
        <Select
          v-model="searchTypeValue"
          style="width:200px"
          :placeholder="$t('common.selectType')"
          class="ml-2"
        >
          <Option v-for="item in typeList" :value="item.id" :key="item.id">{{ item.typeName }}</Option>
        </Select>
        <Select
          v-if="!isGroupIdUse"
          v-model="searchStatusValue"
          style="width:200px"
          :placeholder="$t('common.selectStatus')"
          class="ml-2"
        >
          <Option v-for="item in statusList" :value="item.value" :key="item.value">{{ item.label }}</Option>
        </Select>
        <Input
          v-model="searchValue"
          @on-search="onChangeFillter()"
          search
          class="ml-2 max-width"
          style="height:36px;"
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
          v-for="item in listPolicies"
          :key="item.id"
          :xs="24"
          :sm="12"
          :md="8"
          :lg="8"
        >
          <NewsItem @click.native="viewPolicies(item)" :data="item" />
        </Col> 
      </Row
    ></Card>
    <scroll-loader
      v-if="allPolicies.length < totalCount"
      :loader-method="getMoreData"
      :loader-enable="loadMore"
    >
    </scroll-loader>
  </div>
</template>
<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import PolicyView from "./policyView.vue";
import url from "../../../../lib/url";
import { EPermission } from "../../../../types/information/EPermission";
import SessionStore from "../../../../store/modules/session";
import NewsItem from "../../../../components/news-item.vue";
import { EStatus } from "../../../../types/information/EStatus";
import SessionStorageInfoPage from "../../../../types/session-storage/session-storage";
import IInfoPage from "../../../../types/session-storage/session-storage";
import ScrollLoader from "vue-scroll-loader";
import moment from 'moment';

Vue.use(ScrollLoader);

@Component({
  components: { PolicyView, NewsItem },
  props: {},
  filters: {
    moment(value: any) {
      return moment(value).format("YYYY-MM-DD HH:mm");
    },
  },
})
export default class ListPolicies extends Vue {
  public loadMore: boolean = true;
  public pageSize: number = 8;
  public allPolicies: any[] = [];
  public listPolicies = [];
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

  onChangeFillter() {
    this.currentPage = 1;
    this.fetchData();
  }

  viewPolicies(policies) {
    this.infoPage.pageNumber = this.currentPage;
    SessionStorageInfoPage.save(this.infoPage);
    this.$emit("viewPolicies", policies);
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

  async isUser() {
    const response = await this.$store.dispatch({
      type: "news/init",
    });
    if (response.user.role === 'Employee' || response.user.role === 'Intern') {
      this.isGroupIdUse = true
    }
  }

  async fetchData() {
    this.isUser()
    this.response = await this.$store.dispatch({
      type: "policy/getAllPolicies",
      data: {
        title: this.searchValue,
        pageNumber: this.currentPage,
        pageSize: this.pageSize,
        status: this.searchStatusValue,
        id: this.searchTypeValue,
      },
    });
    this.allPolicies = this.response.items;
    this.totalCount = this.response.totalCount;
    this.listPolicies = [];
    
    this.listPolicies = this.response.items.map((el) => {
      el.isImage = el.image == null ? false : true;
      el.image = url + el.image;
      el.entityName = el.typeName;
      return el;
    });

    this.infoPage.entity = 'Policy';
    this.infoPage.title = this.searchValue;
    this.infoPage.type = this.searchTypeValue;
    this.infoPage.status = this.searchStatusValue;
    this.infoPage.pageNumber = this.currentPage;
  }

  async created() {
    await this.$store
      .dispatch({
        type: "policy/getEntityType",
        entity: "Policy",
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
    if(SessionStorageInfoPage.hasValue('Policy')){
      this.infoPage = SessionStorageInfoPage.getItem('Policy');
      this.searchValue = this.infoPage.title;
      this.searchTypeValue = this.infoPage.type;
      this.searchStatusValue = this.infoPage.status;
      this.currentPage = this.infoPage.pageNumber;
    }
    SessionStorageInfoPage.clear('GuildLine');
    SessionStorageInfoPage.clear('New');
    SessionStorageInfoPage.clear('Event');
    await this.fetchData();
  }

  redirectToEditPolicy() {
    this.infoPage.pageNumber = this.currentPage;
    SessionStorageInfoPage.save(this.infoPage);
    this.$router.push({ name: "createPolicy" });
  }

  getMoreData() {
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
