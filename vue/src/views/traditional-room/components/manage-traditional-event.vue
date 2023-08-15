<template>
  <div>
    <button class="btn create-btn" v-on:click="create()">
      <i class="fas fa-times"></i>Create
    </button>
    <table id="data-table">
      <tr>
        <th>No</th>
        <th>Id</th>
        <th>Title</th>
        <th>Description</th>
        <th>Image</th>
        <th>Album Index</th>
        <th>Album URL</th>
        <th>Album Time</th>
        <th>Parent ID</th>
        <th>Actions</th>
      </tr>
      <tr v-for="(album, index) in allAlbums" :key="album.id">
        <td>{{ index + 1 }}</td>
        <td>{{album.id}}</td>
        <td>
          <textarea
            v-model="album.title"
            :disabled="!album.isEdit"
            v-bind:class="{ editing: !album.isEdit }"
          ></textarea>
        </td>
        <td>
          <textarea
            v-model="album.description"
            :disabled="!album.isEdit"
            v-bind:class="{ editing: !album.isEdit }"
          ></textarea>
        </td>
        <td>
          <textarea v-if="album.isEdit"
            v-model="album.image"
            :disabled="!album.isEdit"
            v-bind:class="{ editing: !album.isEdit }"
          ></textarea>
          <a :href="album.image" v-if="!album.isEdit" target="blank">Image URL</a>
        </td>
        <td>
          <textarea
            v-model="album.albumIndex"
            style="text-align: center"
            :disabled="!album.isEdit"
            v-bind:class="{ editing: !album.isEdit }"
          ></textarea>
        </td>
        <td>
          <textarea v-if="album.isEdit"
            v-model="album.albumUrl"
            :disabled="!album.isEdit"
            v-bind:class="{ editing: !album.isEdit }"
          ></textarea>
          <a :href="album.albumUrl" v-if="!album.isEdit" target="blank">Album URL</a>
        </td>
        <td>
          <!-- <textarea v-model="album.albumTime" :disabled="!album.isEdit" v-bind:class="{editing: !album.isEdit}"></textarea> -->
          <DatePicker
            v-model="album.albumTime"
            type="date"
            placeholder="choose"
            class="datepicker"
            style="width: 100%; text-align: center"
            :disabled="!album.isEdit"
            v-bind:class="{ editing: !album.isEdit }"
          ></DatePicker>
        </td>
        <td>
          <textarea
            v-model="album.parentId"
            style="text-align: center"
            :disabled="!album.isEdit"
            v-bind:class="{ editing: !album.isEdit }"
          ></textarea>
        </td>
        <td>
          <button
            class="btn"
            v-if="!album.isEdit"
            v-on:click="editAlbum(album)"
          >
            Edit
          </button>
          <span v-if="album.isEdit"
            ><img
              src="../../../images/check-icon.png"
              class="edit-icon"
              v-on:click="editOnDone(album)"
              width="24px"
              height="24px"
          /></span>
          <span v-if="album.isEdit"
            ><img
              src="../../../images/close-icon.png"
              class="edit-icon"
              v-on:click="editOnCancel(album)"
              width="24px"
              height="24px"
          /></span>
          <button
            class="btn delete-btn"
            v-if="!album.isEdit"
            v-on:click="deleteAlbum(album)"
          >
            Delete
          </button>
        </td>
      </tr>
    </table>
    <create-album
      v-model="createModalShow"
      @save-success="fetchData"
    ></create-album>
  </div>
</template>
<script lang="ts">
import moment from "moment";
import {
  Component,
  Vue,
  Inject,
  Emit,
  Prop,
  Watch,
} from "vue-property-decorator";
import AbpBase from "../../../lib/abpbase";
import CreateAlbum from "./create-album.vue";

@Component({
  name: "traditional-room",
  components: { CreateAlbum },
  filters: {
    moment(value: any) {
      return moment(value).format("HH:mm | DD-MM-YYYY");
    },
  },
})
export default class Albums extends AbpBase {
  allAlbums = [];
  createModalShow = false;
  async created() {
    this.fetchData();
  }
  async fetchData() {
    let respone = await this.$store.dispatch({
      type: "traditionalRoom/getAllAlbum",
    });
    respone = this.sortByAlbumIndex(respone);
    this.allAlbums = [];
    this.groupAlbumByIndex(respone).map((item) => {
      this.allAlbums.push(item.albums);
      return this.sortByDate(item.albums);
    });
    this.allAlbums = [].concat.apply([], this.allAlbums);
    this.allAlbums.map((item) => (item["isEdit"] = false));
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
  sortByDate(arr) {
    const sorter = (a, b) => {
      return new Date(b.albumTime).getTime() - new Date(a.albumTime).getTime();
    };
    arr.sort(sorter);
    return arr;
  }
  editAlbum(album) {
    album.isEdit = !album.isEdit;
    this.allAlbums = this.allAlbums.map((item) => {
      if (item.id === album.id) {
        return { ...album, isEdit: true };
      }
      return item;
    });
  }
  async editOnDone(album) {
    album.isEdit = false;
    album.albumTime = moment(album.albumTime).format('YYYY-MM-DD[T]HH:mm:ss');
    await this.$store.dispatch({
      type: "traditionalRoom/update",
      data: album,
    });

    this.$Modal.success({
      title: this.L("Confirm"),
      content: this.L("Update album succesfully!"),
    });
    await this.fetchData();
  }
  editOnCancel(album) {
    this.fetchData();
    album.isEdit = false;
  }
  create() {
    this.createModalShow = true;
  }
  async deleteAlbum(album) {
    this.$Modal.confirm({
      title: this.L("Confirm"),
      content: this.L("Delete this album?"),
      okText: this.L("Yes"),
      cancelText: this.L("No"),
      onOk: async () => {
        await this.$store.dispatch({
          type: "traditionalRoom/delete",
          data: album,
        });
        await this.fetchData();
      },
    });
  }
}
</script>
<style lang="scss" scoped>
.editing {
  background: transparent;
  border: none;
  overflow: hidden;
}
table {
  font-family: arial, sans-serif;
  border-collapse: collapse;
  width: 100%;
}
table th {
  font-weight: bold;
  height: 40px;
  white-space: nowrap;
  overflow: hidden;
  background-color: #f8f8f9;
}
td,
th {
  border: 1px solid #dddddd;
  text-align: center;
  padding: 8px;
}
button {
  margin-right: 3px;
  color: #fff;
  background-color: #2d8cf0;
  border-color: #2d8cf0;
  margin-bottom: 5px;
}
.delete-btn {
  color: #fff;
  background-color: #ed4014;
  border-color: #ed4014;
}
.create-btn {
  margin-bottom: 10px;
}
.edit-icon {
  cursor: pointer;
}
textarea {
  resize: none;
}
::v-deep .ivu-select-dropdown {
  top: 8px !important;
}
.datepicker ::v-deep input {
  text-align: center;
}
</style>