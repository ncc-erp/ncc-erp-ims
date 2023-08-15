<template>
  <Modal class="popup" v-model="isShow">
    <div slot="header" class="box-header">
      <h1 class="title">Lịch sử quỹ phạt toàn công ty</h1>
    </div>
    <div class="body-list-comment" style="z-index: 1">
      <Row  style="margin-bottom: 20px">
        <Col span="8">
          <div class="input-search">
            <i
              class="
                ivu-icon
                ivu-icon-ios-loading
                ivu-load-loop
                ivu-input-icon
                ivu-input-icon-validate
              "
            ></i>
            <input
              autocomplete="off"
              spellcheck="false"
              type="text"
              placeholder="Note"
              class="ivu-input ivu-input-default"
              v-model="searchText"
              @keydown.enter="getPage(1)"
            />
            <button
              class="ivu-input-group-append ivu-input-search"
              @click="getPage(1)"
            >
              <span style="margin-left: -7px"
                ><i class="ivu-icon ivu-icon-ios-search"></i
              ></span>
            </button>
          </div>
        </Col>
        <Col span="4">
          <div style="display: flex">
            <div style="margin-left: 20px">
              <Select
                v-model="comparisionValueFilter"
                class="select-data"
                placeholder="Filter by amount"
                @on-change="filterByAmount()"
              >
                <Option
                  v-for="item in OPERATOR_LIST"
                  :value="item.value"
                  :key="item.keySign"
                  >{{ item.keySign }}</Option
                >
              </Select>
            </div>
            <div style="margin-left: 20px">
              <input
                autocomplete="off"
                spellcheck="false"
                type="number"
                placeholder="enter amount value"
                class="ivu-input ivu-input-default input-filter"
                v-model="amountValueFilter"
                @keydown.enter="filterByAmount()"
              />
            </div>
            <div>
                <i class="ivu-icon ivu-icon-ios-close" style="font-size: 32px" @click="clearSearch()"></i>
            </div>
          </div>
        </Col>
      </Row>

      <div class="tableFixed">
        <table id="data-table">
          <thead>
            <th class="col-stt">#</th>
            <th class="col-amount">Amount</th>
            <th class="col-date">Date</th>
            <th>Note</th>
          </thead>
          <tbody>
            <tr v-for="(item, index) in list" :key="index">
              <td class="text-center">{{(currentPage-1)*pageSize+index+1}}</td>
              <td class="text-right amount">
                <span class="mr-2" :class="{ 'negative-amount': item.amount < 0 }">{{ formatMoney(item.amount) }}</span>
              </td>
              <td class="text-center">{{ moment(item.date) }}</td>
              <td class="note">
                <div>{{ item.note }}</div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <Page
          show-sizer
          class-name="fengpage"
          :total="totalCount"
          class="margin-top-10"
          @on-change="pageChange"
          @on-page-size-change="pagesizeChange"
          :page-size="pageSize"
          :current="currentPage"
        ></Page>
    </div>
    <div slot="footer"></div>
  </Modal>
</template>
<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import moment from "moment";
import { Mentionable } from "vue-mention";
@Component({
  components: { Mentionable },
  filters: {
    moment(value: any) {
      return moment(value).format("YYYY-MM-DD HH:mm");
    },
  },
})
export default class FundAmountHistory extends Vue {
  @Prop() private isShow: boolean;
  public listFundAmountHistories: FundAmountHistoryDto[] = [];
  public inputToGetAllPaging = {} as InputToGetAllPagingDto;
  public searchText: string = "";
  public comparisionValueFilter: number = null;
  public amountValueFilter: string = "";
  public filterByAmountObj = null as FilterByComparisonDto;
  public filterComparison = {
    EQUAL: 0,
    LESS_THAN: 1,
    GREATER_THAN: 3,
  };
  public OPERATOR_LIST = {
    [this.filterComparison.LESS_THAN]: {
      keySign: "<",
      name: "Less than",
      value: this.filterComparison.LESS_THAN,
    },
    [this.filterComparison.GREATER_THAN]: {
      keySign: ">",
      name: "Greater than",
      value: this.filterComparison.GREATER_THAN,
    },
    [this.filterComparison.EQUAL]: {
      keySign: "=",
      name: "Equal",
      value: this.filterComparison.EQUAL,
    },
  };
  created() {
    this.$store.state.unlockTimeSheet.currentPage = 1;
    this.$store.state.unlockTimeSheet.pageSize = 10;
    this.getPage(this.currentPage);
  }
  get pageSize() {
    return this.$store.state.unlockTimeSheet.pageSize;
  }
  get totalCount() {
    return this.$store.state.unlockTimeSheet.totalCount;
  }
  get currentPage() {
    return this.$store.state.unlockTimeSheet.currentPage;
  }
  get list() {
    return this.$store.state.unlockTimeSheet.list;
  }

  pageChange(page: number) {
    this.$store.commit("unlockTimeSheet/setCurrentPage", page);
    this.getPage(this.currentPage);
  }
  pagesizeChange(pagesize: number) {
    this.$store.commit("unlockTimeSheet/setPageSize", pagesize);
    this.getPage(this.currentPage);
  }

  moment(value: any) {
    return moment(value).format("DD/MM/YYYY");
  }
  formatMoney(value: any) {
    return new Intl.NumberFormat().format(value);
  }
  public async getPage(page) {
    let param = {
      maxResultCount: this.pageSize,
      skipCount: (page - 1) * this.pageSize,
      searchText: this.searchText,
    } as PagedRequestDto;

    this.inputToGetAllPaging.gridParam = param;
    this.inputToGetAllPaging.filterByComparision = this.filterByAmountObj;

    await this.$store.dispatch({
      type: "unlockTimeSheet/getFunAmountHistories",
      data: this.inputToGetAllPaging,
    });
  }

  public filterByAmount() {
    if (
      this.comparisionValueFilter != null &&
      this.amountValueFilter != "" &&
      this.amountValueFilter != "-"
    ) {
      this.filterByAmountObj = {
        operatorComparison: this.comparisionValueFilter,
        value: this.amountValueFilter,
      };
    } else {
      this.filterByAmountObj = null;
    }
    this.getPage(1);
  }

  public clearSearch(){
    this.searchText = "";
    this.comparisionValueFilter = null;
    this.amountValueFilter = "";
    this.filterByAmountObj = null;
    this.getPage(1);
  }
}
export class FundAmountHistoryDto {
  id: number;
  amount: number;
  date: string;
  note: string;
}
export class PagedRequestDto {
  skipCount: number;
  maxResultCount: number;
  searchText: string;
  sort: string;
  sortDirection: number;
}
export class FilterDto {
  propertyName: string;
  value: any;
  comparision: number;
  filterType?: number;
  dropdownData?: any[];
}
export class InputToGetAllPagingDto {
  gridParam: PagedRequestDto;
  filterByComparision: FilterByComparisonDto;
}
export class FilterByComparisonDto {
  operatorComparison: number;
  value: string;
}
</script>
<style lang="scss" scoped>
::v-deep .ivu-modal {
  width: 70% !important;
}
::v-deep .ivu-modal-body {
  width: 100% !important;
  max-height: 65%;
}
::v-deep .body-list-comment {
  height: 100%;
}
::v-deep .ivu-modal-header {
  padding: 25px 20px 5px !important;
}
::v-deep .ivu-modal-close i {
  margin-top: 10px;
}
.title {
  text-align: left;
}
table {
  background-color: transparent;
  width: 100% !important;
}
th,
td {
  border: 1px solid #e8eaec;
  height: 50px;
}
th {
  text-align: center;
}
.col-stt {
  width: 50px;
}
.col-amount {
  width: 150px;
}
.col-date {
  width: 150px;
}
.amount span {
  margin-right: 10px;
}
.note div {
  padding: 10px;
  word-wrap: break-word;
  white-space: pre-line;
  min-width: 300px;
}
.input-search {
  display: flex;
}
.tableFixed {
  max-height: 625px;
  overflow: auto;
}
.tableFixed thead {
  background: #f8f9fa;
  position: sticky;
  top: -1px;
  z-index: 2;
}
.select-data{
    width: 150px;
}
.input-filter{
    width: 200px;
}
.negative-amount{
    color: #DC3545;
}
</style>
