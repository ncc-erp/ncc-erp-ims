<template>
  <!-- <div>
    <div class="manage-room">
      <button
        class="btn"
        v-if="SessionStore.state.user.role == 'Admin'"
        v-on:click="onCreateEvent"
        ghost
      >
        Manage traditional room
      </button>
    </div>
    <div
      class="event-row"
      v-for="parentAlbum in parentAlbums"
      :key="parentAlbum.id"
    >
      <div class="title">
        <span>{{ parentAlbum.title }}</span>
      </div>
      <div class="photo-gallery">
        <div
          class="pic"
          v-for="event in get(childAlbums, parentAlbum.id)"
          :key="event.id"
        >
          <a v-bind:href="event.albumUrl" target="_blank">
            <img v-bind:src="event.image" alt="" />
          </a>
          <div class="details">
            <div class="meta">
              <div class="meta-left tooltip" style="opacity: 1">
                <h3>
                  {{event.title}}
                </h3>
                 
              </div>
            </div>
            <div class="meta-right">
              <i class="fa fa-ellipsis-v"></i>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div> -->

  <div>
    <div class="manage-room">
      <button
        class="btn"
        v-if="SessionStore.state.user.role == 'Admin'"
        v-on:click="onCreateEvent"
        ghost
      >
        Manage traditional room
      </button>
    </div>
    <div v-for="parentAlbum in parentAlbums"
      :key="parentAlbum.id" >
      <h3 class="title-left">
            {{ parentAlbum.title }}
          </h3>
      <div class="grid-videos" >
        <div class="single-video" v-for="event in get(childAlbums, parentAlbum.id)"
          :key="event.id">
          <div style="width: 100%; height: 230px;">
            <a v-bind:href="event.albumUrl" target="_blank">
            <img
              v-bind:src="event.image"
              alt=""
              class="thumbnail-image"
            />
          </a>
          </div>
          
          <div class="details">
            <div class="meta">
              <div class="meta-left tooltip" style="opacity: 1">
                <h3 class="title-post">
                  {{event.title}}
                </h3>
                <span class="tooltiptext">{{event.title}}</span>
              </div>
            </div>
            <div class="meta-right">
              <i class="fa fa-ellipsis-v"></i>
            </div>
          </div>
        </div>
      </div>
    </div>
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
import SessionStore from "../../store/modules/session";
@Component({
  name: "traditional-room",
  components: {},
})
export default class ListBlog extends Vue {
  childAlbums: any = [];
  parentAlbums = [];
  allAlbums = [];
  private SessionStore = SessionStore;
  title = "Traditional Room";
  async created() {
    this.fetchData();
  }
  async fetchData() {
    let respone = await this.$store.dispatch({
      type: "traditionalRoom/getAllAlbum",
    });
    respone = this.sortByAlbumIndex(respone);
    this.groupAlbumByIndex(respone).map((item) => {
      this.allAlbums.push(item.albums);
      return this.sortByDate(item.albums);
    });
    respone = [].concat.apply([], this.allAlbums);
    this.childAlbums = respone.filter((item) => item.parentId != null);
    this.parentAlbums = respone.filter((item) => item.parentId == null);
  }
  get(arr, parentId) {
    return Array.from(arr).filter((item: any) => item.parentId == parentId);
  }
  sortByDate(arr) {
    const sorter = (a, b) => {
      return new Date(b.albumTime).getTime() - new Date(a.albumTime).getTime();
    };
    arr.sort(sorter);
    return arr;
  }
  sortByAlbumIndex(arr) {
    const sorter = (a, b) => {
      return b.albumIndex - a.albumIndex;
    };
    arr.sort(sorter);
    return arr;
  }
  groupAlbumByIndex(arr) {
    var object = {};
    var results = arr.reduce(function (res, ele, index) {
      var e = ele.albumIndex;
      if (!object[e]) {
        object[e] = {
          albumIndex: ele.albumIndex,
          albums: [],
        };
        res.push(object[e]);
      }
      object[e].albums.push(ele);
      return res;
    }, []);
    return results;
  }
  onCreateEvent() {
    this.$router.push("/traditional-room/manage-traditional-room");
  }
}
</script>
<style lang="scss" scoped>

.manage-room {
  display: flex;
  justify-content: flex-end;
  padding: 10px;
}

.title-post {
  border-left: 3px solid rgb(255, 115, 115);
  color: #000;
}

.grid-videos {
  display: flex;
  flex-wrap: wrap;
  justify-content: flex-start;
}
.single-video {
  width: calc(25% - 16px);
  margin: 0 8px;
  margin-bottom: 10px;
}
.thumbnail-image {
  display: block;
  width: 100%;
  height: 100%;
  object-fit: cover;
}
.video-bottom-section {
  display: flex;
  max-width: 253px;
}
.video-title {
  overflow: hidden;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-line-clamp: 2; /* number of lines to show */
  -webkit-box-orient: vertical;
  text-decoration: none;
  color: #030303;
  font-weight: 500;
}
.channel-icon {
  margin-right: 0.75rem;
  border-radius: 50%;
  background: #aaa;
}

/*RESPONSIVE*/
@media only screen and (min-width: 1126px) {
  #dark-effect {
    width: 0;
  }
  #sidebar-slider {
    display: none !important;
  }
}
@media only screen and (max-width: 1125px) {
  .header .start {
    width: 210px;
  }
  .header .center {
    margin-left: 0;
  }
  .nav-right {
    margin-left: 83px;
  }

  .single-video {
    width: calc(33% - 16px);
  }
  .nav-left {
    display: none;
  }
  .nav-left-hide {
    display: block !important;
  }
}

@media only screen and (max-width: 887px) {
  .single-video {
    width: calc(44% - 22px);
  }
  .grid-videos {
    justify-content: center;
  }
}
@media only screen and (max-width: 808px) {
  .nav-left {
    display: none;
  }
  .nav-right {
    margin-left: 0 !important;
  }
  .blur-div {
    left: 721px;
  }
  .right-slider {
    left: 741px;
  }
  .nav-left-hide {
    display: none !important;
  }
}
@media only screen and (max-width: 676px) {
  .header .center {
    width: 34%;
  }
  .search-box input {
    max-width: 94px;
  }
}
@media only screen and (max-width: 670px) {
  .search-box input {
    display: none;
  }
  .header .end {
    width: 30%;
  }
  .search-box button {
    background: transparent !important;
    border: none !important;
  }
}
@media only screen and (max-width: 513px) {
  .single-video {
    width: calc(76% - 23px);
  }
  .blur-div {
    left: 341px;
  }
  .right-slider {
    left: 375px;
  }
}
@media only screen and (max-width: 513px) {
  .grid-videos {
    padding: 0;
  }
  .single-video {
    width: 320px;
  }
  .search-box i {
    display: none;
  }
  .bell {
    display: none;
  }
  .header .end {
    width: 40%;
    justify-content: space-between;
  }
}

.channel-profile {
  height: 36px;

  margin-right: 12px;
}
.channel-profile img {
  border-radius: 50%;
  background-color: transparent;
  overflow: hidden;
}
.details {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  margin-top: 10px;
}
.meta-left h3 {
  font-size: 15px;
  font-weight: 700;
  margin-bottom: 10px;
  overflow: hidden;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  padding: 0px 3px;
}
.channel-name p,
.views {
  font-size: 15px;
  color: #606060;
}
.tooltip {
  position: relative;
}

.tooltip .tooltiptext {
  font-size: 13px;
  visibility: hidden;
  width: auto;
  background: #fff;
  text-align: center;
  padding: 3px 0;
  position: absolute;
  z-index: 1;
  top: 33%;
  left: 22%;
  white-space: nowrap;
  border: 1px solid #000;
  padding: 4px;
  z-index: 1;
}

.tooltip:hover .tooltiptext {
  visibility: visible;
}

.title-left {
    text-align: left;
    font-size: 40px;
    margin-bottom: 22px;
    font-weight: bold;
    text-transform: uppercase;
    color: indianred;
    position: relative;
    padding-bottom: 6px;
    padding-left: 20px;
  }
  .title-left::before {
    height: 1px;
    display: block;
    margin-top: 3px;
    width: 100%;
    background: #ccc;
    content: '';
    position: absolute;
    left: 0;
    bottom: 0;
  }
</style>