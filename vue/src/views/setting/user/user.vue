<template>
  <div>
    <Card dis-hover>
      <div class="page-body">
        <Form ref="queryForm" :label-width="80" label-position="left" inline>
          <Row :gutter="16">
            <Col span="6">
              <FormItem :label="L('SearchText') + ':'" style="width:100%">
                <Input
                  enter-button
                  search
                  @on-search="getpage(1)"
                  v-model="pagerequest.SearchText"
                  :placeholder="L('UserName') + '/' + L('Name')"
                ></Input>
              </FormItem>
            </Col>
            <Col span="6">
              <FormItem :label="L('IsActive') + ':'" style="width:100%">
                <!--Select should not set :value="'All'" it may not trigger on-change when first select 'NoActive'(or 'Actived') then select 'All'-->
                <Select :placeholder="L('Select')" v-model="isActive">
                  <Option value="" selected>{{ L("All") }}</Option>
                  <Option value="true">{{ L("Actived") }}</Option>
                  <Option value="false">{{ L("NoActive") }}</Option>
                </Select>
              </FormItem>
            </Col>
            <Col span="6">
              <FormItem :label="L('CreationTime') + ':'" style="width:100%">
                <DatePicker
                  v-model="creationTime"
                  type="datetimerange"
                  format="yyyy-MM-dd"
                  style="width:100%"
                  placement="bottom-end"
                  :placeholder="L('SelectDate')"
                ></DatePicker>
              </FormItem>
            </Col>
          </Row>
          <Row>
            <Button
              @click="create"
              icon="android-add"
              type="primary"
              size="large"
              >{{ L("Add") }}</Button
            >
            <Button
              icon="ios-search"
              type="primary"
              size="large"
              @click="getpage(1)"
              class="toolbar-btn"
              >{{ L("Find") }}</Button
            >
          </Row>
        </Form>
        <div class="margin-top-10">
          <Table
            :loading="loading"
            :columns="columns"
            :no-data-text="L('NoDatas')"
            border
            :data="list"
          >
          </Table>
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
      </div>
    </Card>
    <create-user
      v-model="createModalShow"
      @save-success="getpage(currentPage)"
    ></create-user>
    <edit-user
      v-model="editModalShow"
      @save-success="getpage(currentPage)"
    ></edit-user>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import Util from "@/lib/util";
import AbpBase from "@/lib/abpbase";
import PageRequest from "@/store/entities/page-request";
import CreateUser from "./create-user.vue";
import EditUser from "./edit-user.vue";
class PageUserRequest extends PageRequest {
  SearchText: string;
}
@Component({
  components: { CreateUser, EditUser },
})
export default class Users extends AbpBase {
  edit() {
    this.editModalShow = true;
  }
  //filters
  pagerequest: PageUserRequest = new PageUserRequest();
  creationTime: Date[] = [];
  fromDate: string = "";
  toDate: string = "";
  isActive: string = "";
  createModalShow: boolean = false;
  editModalShow: boolean = false;
  get list() {
    return this.$store.state.user.list;
  }
  get loading() {
    return this.$store.state.user.loading;
  }
  create() {
    this.createModalShow = true;
  }
  pageChange(page: number) {
    this.$store.commit("user/setCurrentPage", page);
    this.getpage(this.currentPage);
  }
  pagesizeChange(pagesize: number) {
    this.$store.commit("user/setPageSize", pagesize);
    this.getpage(this.currentPage);
  }
  async getpage(page: number) {
    this.pagerequest.maxResultCount = this.pageSize;
    this.pagerequest.skipCount = (page - 1) * this.pageSize;
    //filters

    if (this.creationTime.length > 0) {
      this.fromDate = this.convertDate(this.creationTime[0]);
    }
    if (this.creationTime.length > 1) {
      this.toDate = this.convertDate(this.creationTime[1]);
    }
    if (this.isActive == "") this.isActive == null;

    await this.$store.dispatch({
      type: "user/getAll",
      data: {
        value: this.pagerequest,
        isActive: this.isActive,
        fd: this.fromDate,
        td: this.toDate,
      },
    });
  }
  get pageSize() {
    return this.$store.state.user.pageSize;
  }
  get totalCount() {
    return this.$store.state.user.totalCount;
  }
  get currentPage() {
    return this.$store.state.user.currentPage;
  }
  columns = [
    {
      title: this.L("UserName"),
      key: "userName",
    },
    {
      title: this.L("Name"),
      key: "name",
    },
    {
      title: this.L("IsActive"),
      render: (h: any, params: any) => {
        return h("span", params.row.isActive ? this.L("Yes") : this.L("No"));
      },
    },
    {
      title: this.L("FullName"),
      key: "fullName",
    },
    // {
    //   title: this.L("KomuUserName"),
    //   key: "komuUserName",
    // },
    {
      title: this.L("UserCode"),
      key: "userCode",
    },
    {
      title: this.L("CreationTime"),
      key: "creationTime",
      render: (h: any, params: any) => {
        return h(
          "span",
          new Date(params.row.creationTime).toLocaleDateString()
        );
      },
    },
    {
      title: this.L("LastLoginTime"),
      render: (h: any, params: any) => {
        return h("span", new Date(params.row.lastLoginTime).toLocaleString());
      },
    },
    {
      title: this.L("Actions"),
      key: "Actions",
      width: 150,
      render: (h: any, params: any) => {
        return h("div", [
          h(
            "Button",
            {
              props: {
                type: "primary",
                size: "small",
              },
              style: {
                marginRight: "5px",
              },
              on: {
                click: () => {
                  this.$store.commit("user/edit", params.row);
                  this.edit();
                },
              },
            },
            this.L("Edit")
          ),
          h(
            "Button",
            {
              props: {
                type: "error",
                size: "small",
              },
              on: {
                click: async () => {
                  this.$Modal.confirm({
                    title: this.L("Tips"),
                    content: this.L("DeleteUserConfirm"),
                    okText: this.L("Yes"),
                    cancelText: this.L("No"),
                    onOk: async () => {
                      await this.$store.dispatch({
                        type: "user/delete",
                        data: params.row,
                      });
                      await this.getpage(this.currentPage);
                    },
                  });
                },
              },
            },
            this.L("Delete")
          ),
        ]);
      },
    },
  ];
  convertDate(date: Date) {
    console.log(date);
    if (date) {
      return (
        this.addZeroDate(date.getUTCMonth() + 1) +
        "/" +
        this.addZeroDate(date.getDate()) +
        "/" +
        date.getUTCFullYear()
      );
    }
    return "";
  }
  addZeroDate(value) {
    console.log(value);
    if (value < 10) return "0" + value.toString();
    return value;
  }
  async created() {
    this.getpage(this.currentPage);
    await this.$store.dispatch({
      type: "user/getRoles",
    });
  }
}
</script>
