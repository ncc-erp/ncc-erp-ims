<template>
  <div>
    <Row>
      <Col :xs="24" :sm="18" :md="18" :lg="18">
        <Card v-if="listBlogs.length == 0">Hiện tại, không có tin nào ! </Card>
        <Card
          class="list-item"
          style="margin: auto; margin-top: 10px"
          v-for="(item, index) in listBlogs"
          :key="index"
        >
          <Row type="flex" justify="space-between">
            <Col span="12" class="list-item-left">
              <div class="custom-img-box"></div>
            </Col>
            <Col span="24" class="list-item-right">
              <div class="user-item">
                <img
                  class="img-item"
                  v-bind:src="item.urlAvatar"
                  alt="img"
                  height="60"
                />
                <div>
                  <h4 class="title" @click="onDetailBlog(item.postId)">
                    {{ item.tilte }}
                  </h4>
                  <span
                    class="glyphicon glyphicon-user"
                    style="margin-right: 5px"
                  ></span
                  ><span style="margin-right: 10px"
                    ><strong @click="openBlog(item.userId)">{{
                      item.fullName
                    }}</strong></span
                  >
                  <span>{{ item.timeCreated }}</span>
                  <span
                    class="glyphicon glyphicon-thumbs-up"
                    style="margin-left: 10px"
                  ></span
                  ><span>{{ item.totalReaction || 0 }}</span>
                  <span
                    class="glyphicon glyphicon-comment"
                    style="margin-left: 10px"
                  ></span
                  ><span>{{ item.totalComment || 0 }}</span>
                </div>
              </div>
              <hr width="100%" />

              <p class="item-detail" v-html="item.detail"></p>
              <span
                ><a
                  style="color: blue; margin-top: 10px; float: right"
                  @click="onDetailBlog(item.postId)"
                  >Xem chi tiết...
                </a></span
              >
              <div style="height: 10px"></div>
            </Col>

            <hr width="100%" />

            <Col span="24" style="text-align: right; margin-top: 15px">
              <span v-if="item.reaction == null"
                >{{ item.totalReaction
                }}<Icon
                  @click="onLikeBlog(item.postId, item, index)"
                  class="icono-thumbs-up"
                  type="md-thumbs-up"
              /></span>
              <span v-else class="thumbs-up-on"
                >{{ item.totalReaction
                }}<Icon
                  @click="onLikeBlog(item.postId, item, index)"
                  class="icono-thumbs-up"
                  type="md-thumbs-up"
              /></span>
            </Col>
          </Row>
        </Card>
      </Col>
      <Col :xs="24" :sm="6" :md="6" :lg="6">
        <Card style="margin-top: 10px; margin-left: 10px">
          <h5>Tìm kiếm hashtag</h5>
          <hr width="100%" />
          <Row class="row mb-2 search">
            <Col
              span="6"
              class="col col-label text-right col__item title-left-dd"
            >
              Nhập từ khóa
            </Col>
            <Col span="18" class="col col__item">
              <Input v-model="formSearch.hashtag" />
            </Col>
            <Col
              offset="6"
              span="18"
              class="col col__item"
              style="margin-top: 10px"
            >
              <Button type="primary" @click="searchHashTag(formSearch.hashtag)"
                ><span>Tìm kiếm</span></Button
              >
            </Col>
          </Row>
          <hr width="100%" />
          <div style="margin-top: 10px">
            <h4>Top 10 HashTag</h4>
            <ul
              class="tophashtag"
              v-for="(item, index) in listTop10"
              :key="index"
            >
              <li>
                <h4 @click="searchHashTag(item.hashtag)">{{ item.hashtag }}</h4>
                <span>{{ item.usedTimes }} bài viết</span>
              </li>
            </ul>
          </div>
        </Card>
      </Col>
      <!----->
    </Row>
    <scroll-loader
      v-if="allBlogs.length < totalCount"
      :loader-method="getImagesInfo"
      :loader-enable="loadMore"
    >
    </scroll-loader>
  </div>
</template>
<script lang="ts">
import {
  Component,
  Vue,
  Inject,
  Emit,
  Prop,
  Watch,
} from "vue-property-decorator";
import IInfoPage from "../../../../types/session-storage/session-storage";
import BlogModule from "@/store/modules/blog";
import moment from "moment";
import ScrollLoader from "vue-scroll-loader";
import url from "../../../../lib/url";
import { IFormSearch } from "@/types/blog/createEditBlog";
//import SessionStorageInfoPage from '../../../types/session-storage/session-storage'
Vue.use(ScrollLoader);
@Component({
  components: {},
})
export default class ViewListBlog extends Vue {
  private loadMore: boolean = true;
  private isListTag = false;
  private pageSize: number = 4;
  private allBlogs: any = [];
  private listBlogs = [];
  private listBlogOfHashtag = [];
  private listTop10 = [];
  private response: any = {};
  private currentPage = 1;
  private totalCount = 1;
  private searchValue: string = "";
  private searchTypeValue: string = "";
  private searchStatusValue: string = "";
  private isSearch = false;
  // private SessionStore = SessionStore;
  private statusList = [];
  private typeList = [];
  private infoPage = new IInfoPage();
  private formSearch = {} as IFormSearch;
  private isGroupIdUse: boolean = false;
  private HASHTAG: string;

  async fetchData() {
    if (this.isSearch == true) {
      this.searchHashTag(this.HASHTAG);
      return;
    }
    this.isListTag = false;
    let response = await this.$store.dispatch({
      type: "blog/getAllBlogFeed",
      data: {
        isInBlogFeed: 1,
        pageNumber: 1,
        pageSize: this.pageSize,
      },
    });
    let top10Hashtag = await this.$store.dispatch({
      type: "blog/top10Hashtag",
    });
    this.listTop10 = top10Hashtag;
    this.allBlogs = response.items;
    this.totalCount = response.totalCount;
    this.listBlogs = response.items.map((el) => {
      return {
        ...el,
        timeCreated: moment(el.timeCreated).format("DD-MM-YYY"),
        urlAvatar: url + el.urlAvatar,
        detail: this.getHasshtag(el.detail),
      };
    });
  }

  getHasshtag(data) {
    let detail = "";
    let before = data.slice(0, data.indexOf("#")); // chuỗi trc hashtag
    let after = data.slice(data.indexOf("#"), data.length); // chuỗi còn lại
    let final = after.slice(after.indexOf("#"), after.indexOf(" ")); // chuỗi hashtag
    let afterfinal = after.slice(final.length, after.length); // chuỗi sau hashtag
    if (afterfinal.includes("#")) {
      afterfinal = this.getHasshtag(afterfinal);
    }
    let hashtag;
    hashtag = final.link("#");
    detail = before + hashtag.bold() + afterfinal;
    return detail;
  }

  private async searchHashTag(hashtag) {
    this.HASHTAG = hashtag;
    this.isSearch = true;
    let response = await this.$store.dispatch({
      type: "blog/findHashtag",
      data: {
        hashtag: hashtag,
        pageSize: this.pageSize,
        pageNumber: 1,
      },
    });

    if (response.items.length !== 0) {
      this.listBlogOfHashtag = response.items.map((el) => {
        return {
          ...el,
          timeCreated: moment(el.timeCreated).format("DD-MM-YYYY"),
          urlAvatar: url + el.urlAvatar,
          detail: this.getHasshtag(el.detail),
        };
      });
      this.listBlogs = [];
      this.listBlogs = this.listBlogOfHashtag;
    } else {
      this.isSearch = false;
      this.fetchData();
    }
  }

  onDetailBlog(id) {
    this.$emit("onDetailBlog", id);
  }

  getImagesInfo() {
    this.pageSize = this.pageSize + 4;
    this.fetchData();
  }

  private async onLikeBlog(id, item, index) {
    let formData = {
      postId: id,
      reaction: 1,
    };
    let response = await this.$store.dispatch({
      type: "blog/reactionPost",
      data: formData,
    });
    this.listBlogs[index].reaction = response.reaction;
    this.listBlogs[index].totalReaction = response.totalReact;
  }

  openBlog(id: any) {
    this.$router.push({ name: "userBlog", params: { userId: id } });
  }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
.thumbs-up-on {
  color: blue;
}
.icono-thumbs-up {
  width: 20px;
}
.tophashtag {
  list-style-type: none;
  margin-left: 15px;
  h4 {
    color: blue;
    margin-top: 10px;
    cursor: pointer;
  }
}
.search {
  margin-top: 15px;
  margin-bottom: 25px;
}
.img-item {
  width: 100px;
  height: 100px;
  margin-right: 15px;
  border-radius: 50%;
}
.title {
  padding: 10px 0px;
  font-weight: bold;
  color: blue;
}
.user-item {
  //
  padding-bottom: 10px;
  display: flex;
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
.title:hover {
  color: deepskyblue;
}
</style>