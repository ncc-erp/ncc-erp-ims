<template>
  <div>
    <Modal
      :title="L('Create Album')"
      :value="value"
      @on-ok="save"
      @on-visible-change="visibleChange"
    >
      <Form ref="roleForm" label-position="top" :model="album">
        <FormItem :label="L('Title')" prop="title">
          <Input v-model="album.title"  :minlength="2"></Input>
        </FormItem>
        <FormItem :label="L('Description')" prop="description">
          <Input
            v-model="album.description"
            :minlength="2"
          ></Input>
        </FormItem>
        <FormItem :label="L('Image URL')" prop="image">
          <Input v-model="album.image" ></Input>
        </FormItem>
        <FormItem :label="L('Album index')" prop="albumIndex">
          <Input v-model="album.albumIndex" ></Input>
        </FormItem>
        <FormItem :label="L('Album URL')" prop="albumUrl">
          <Input v-model="album.albumUrl" ></Input>
        </FormItem>
        <FormItem :label="L('Album time')" prop="albumTime">
          <!-- <Input v-model="album.albumTime"></Input> -->
          <DatePicker
            v-model="album.albumTime"
            type="date"
            placeholder="Choose a date"
            style="width: 100%; text-align: center"
          ></DatePicker>
        </FormItem>
        <FormItem :label="L('Parent ID')" prop="parentId">
          <Input v-model="album.parentId" ></Input>
        </FormItem>
      </Form>
      <div slot="footer">
        <Button @click="cancel">{{ L("Cancel") }}</Button>
        <Button @click="save" type="primary">{{ L("OK") }}</Button>
      </div>
    </Modal>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from "vue-property-decorator";
import Util from "../../../lib/util";
import AbpBase from "../../../lib/abpbase";
import TraditionalRoom from "../../../store/entities/traditionalRoom";
import moment from "moment";

@Component
export default class CreateAlbum extends AbpBase {
  @Prop({ type: Boolean, default: false }) value: boolean;
  album: TraditionalRoom = new TraditionalRoom();
  async save() {
    this.album.albumTime = moment(this.album.albumTime).format('YYYY-MM-DD[T]HH:mm:ss');
    await this.$store.dispatch({
      type: "traditionalRoom/create",
      data: this.album,
    });
    (this.$refs.roleForm as any).resetFields();
    this.$emit("save-success");
    this.$emit("input", false);
  }
  cancel() {
    (this.$refs.roleForm as any).resetFields();
    this.$emit("input", false);
  }
  visibleChange(value: boolean) {
    if (!value) {
      this.$emit("input", value);
    }
  }
}
</script>
<style lang="scss" scoped>
::v-deep .ivu-modal {
  width: 666px !important;
  top: 10px;
}
</style>