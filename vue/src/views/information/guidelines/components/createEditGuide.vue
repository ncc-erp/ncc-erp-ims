<template>
  <div class="edit-policies" :loading="loading">
    <h2>{{isCreate ? $t("guide.createPageTitle") : $t("guide.editPageTitle")}}</h2>
    <Divider />
    <Row v-if="(isEdit && guide.canDelete) || (SessionStore.state.user.role == 'Admin' && !isCreate)" class="form-row mb-2">
      <Col span="2" offset="22">
        <Button type="error" class="btn btn--delete" @click="deleteEvent" style="float: right;">{{ $t("policy.delete")}}</Button>
      </Col>
    </Row>
    <Row>
      <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("policy.title")}}
          </Col>
          <Col span="20" class="col col__item">
            <Input v-model="guide.title" />
            <span
              v-if="!guide.title && isClickSubmit"
              class="error-text"
            >{{$t("common.errorEmpty", { field: $t("policy.title")})}}</span>
          </Col>
        </Row>
      </Col>

      <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("policy.shortDescription")}}
          </Col>
          <Col span="20" class="col col__item">
            <Input
              v-model="guide.shortDescription"
              :maxlength="500"
              type="textarea"
              :autosize="{minRows: 2,maxRows: 3}"
            />
            <span
              v-if="!guide.shortDescription && isClickSubmit"
              class="error-text"
            >{{$t("common.errorEmpty", { field: $t("policy.shortDescription")})}}</span>
          </Col>
        </Row>
      </Col>

      <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("policy.avatar")}}
          </Col>
          <Col span="20" class="col col__item">
            <input class="input-file" type="file" @change="handleInputFile" accept="image/*" />
            <p v-if="editIMG">{{ editIMG }}</p>
          </Col>
        </Row>
      </Col>

      <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("common.coverImage")}}
          </Col>
          <Col span="20" class="col col__item">
              <input type="file" @change="croppie"/>
              <span v-if="!urlIMG">
                <div v-if="isCoverImage">
                  <img v-bind:src="cropped">
                </div>
                <div v-else><vue-croppie ref="croppieRef" :enableOrientation="true" :boundary="{ width: 890, height: 300}" :viewport="{ width:835, height:150, 'type':'square' }"
                @result="result">
                </vue-croppie></div>
                <div align="center">
                <Button v-if="isCrop && !isCoverImage" @click="crop" type="primary">{{ $t("common.crop")}}</Button>
                </div>
              </span>
              <span v-if="urlIMG"><img :src="urlIMG"></span>
          </Col>
        </Row>
      </Col>

      <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("policy.description")}}
          </Col>
          <Col span="20" class="col col__item">
            <Editor :value="guide.description" @onChange="handleDescriptionChange" />
            <span
              v-if="!guide.description && isClickSubmit"
              class="error-text"
            >{{$t("common.errorEmpty", { field: $t("policy.description")})}}</span>
          </Col>
        </Row>
      </Col>

      <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("policy.type")}}
          </Col>
          <Col span="8" class="col col__item">
            <Select v-model="guide.entityTypeId" style="width:200px">
              <Option
                v-for="item in this.types"
                :value="item.id"
                :key="item.id"
              >{{ item.typeName }}</Option>
            </Select>
            <span
              v-if="!guide.entityTypeId && isClickSubmit"
              class="error-text"
            >{{$t("common.errorEmpty", { field: $t("policy.type")})}}</span>
          </Col>
        </Row>
      </Col>

      <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("policy.group")}}
          </Col>
          <Col span="8" class="col col__item">
            <Select v-model="guide.groupId" style="width:200px">
              <Option
                v-for="item in this.groups"
                :value="item.value"
                :key="item.value"
              >{{ item.label }}</Option>
            </Select>
            <span
              v-if="this.guide.groupId !== 0 && !guide.groupId && isClickSubmit"
              class="error-text"
            >{{$t("common.errorEmpty", { field: $t("policy.group")})}}</span>
          </Col>
        </Row>
      </Col>

      <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("policy.priority")}}
          </Col>
          <Col span="8" class="col col__item">
            <Select v-model="guide.piority" style="width:200px">
              <Option
                v-for="item in this.priorities"
                :value="item.value"
                :key="item.value"
              >{{ item.label }}</Option>
            </Select>
            <span
              v-if="!guide.piority && isClickSubmit"
              class="error-text"
            >{{$t("common.errorEmpty", { field: $t("policy.priority")})}}</span>
          </Col>
        </Row>
      </Col>
      <Col class="col" span="24">
        <div class="text-center mt-4">
          <Button class="btn btn--create" type="primary" @click="handleSubmit"><p v-if="!isEdit">{{$t("guide.createPageTitle")}}</p> <p v-else>{{$t("guide.editPageTitle")}}</p></Button>
          <Button class="btn btn--back ml-2" @click="cancelGuide">{{$t("common.cancel")}}</Button>
        </div>
      </Col>
    </Row>
  </div>
</template>
<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import editorComponent from "../../../tinymce/editorComponent.vue";
import Iguide from "../../../../store/entities/policy";
import SessionStore from '../../../../store/modules/session'
import url from '../../../../lib/url';
import VueCroppie from 'vue-croppie'
import 'croppie/croppie.css'

Vue.config.productionTip = false
Vue.use(VueCroppie)

@Component({
  components: {
    Editor: editorComponent
  }
})
export default class CreateEditGuide extends Vue {
  private isEdit: boolean = false
  private editIMG: string = ''
  private guide: any = {
    effectiveStartTime: "",
    effectiveEndTime: "",
    title: "",
    description: "",
    groupId: 0,
    piority: 1,
    entityTypeId: 1,
    shortDescription: ""
  };
  private types: any = [];
  private groups: any = [
    {
      value: 0,
      label: "Tất cả"
    }
  ];
  private priorities: any = [
   {
      value: 1,
      label: "Quan trọng"
    },
    {
      value: 2,
      label: "Cao"
    },
    {
      value: 3,
      label: "Thấp"
    }
  ];
  private isClickSubmit: boolean = false;
  private loading: boolean = false;
  private isCreate: boolean = true;
  private SessionStore = SessionStore
  private coverImage: string = ''
  private isCoverImage: boolean = false
  private croppieImage: string = ''
  private isCrop: boolean = false
  private cropped: any = null
  private urlIMG: string = ''


  async crop() {
      let options = {
        type: 'base64',
        size: { width: 1635, height: 300 },
        format: 'jpeg'
      };
      let ref: any = this.$refs.croppieRef
      await ref.result(options, (output) => {
        this.cropped = output
      })
        this.isCoverImage = true
        this.guide.coverImage = this.cropped.replace("data:image/jpeg;base64,","");
  }

  result (output) {
    this.guide.coverImage = output
  }

  croppie (e) {
    this.isCoverImage = false
    this.isCrop = true
      let files = e.target.files || e.dataTransfer.files;
      if (!files.length) return;

      let reader = new FileReader();
      reader.onload = e => {
        let ref: any = this.$refs.croppieRef
        ref.bind({
          url: e.target.result
        });
      };

    reader.readAsDataURL(files[0]);
    this.urlIMG = ''
  }

  async created() {
    const entity: string = 'GuildLine'
      const entityType = await this.$store.dispatch({
          type: "news/getEntityType",
          entity: entity
        })
    this.types = entityType.result
    if (this.$route.params.guideId) {
      this.isEdit = true
      this.loading = true;
      this.isCreate = false;
      let response = await this.$store.dispatch({
        type: "guide/getGuide",
        id: this.$route.params.guideId
      });
      this.editIMG = response.image
      this.urlIMG = url + response.coverImage
      this.guide = response
      this.guide.coverImage = null
      this.guide = {...this.guide}
      this.loading = false;
    }
  }

  handleDescriptionChange(value: string) {
    this.guide.description = value;
  }

  async handleInputFile(guide: any) {
    this.guide.Image = guide.target.files[0];
    if (this.guide.Image) {
      this.editIMG = ''
    }
  }

  async handleSubmit() {
    this.isClickSubmit = true;
    if (this.checkValidate()) {
      let file = new FormData();
      for (let key in this.guide) {
        if (key === 'coverImage') {
          if (this.guide[key]) {
            file.append(key, this.guide[key]);
          }
        } else file.append(key, this.guide[key]);
      }

      let response = await this.$store.dispatch({
        type: "guide/createEditGuide",
        dataGuide: file
      });
      if (this.isEdit) {
        this.$router.push(`/information/guidelines/${this.$route.params.guideId}`);
      } else {
        this.$router.push(`/information/guidelines/${response.id}`);
      }
      this.isClickSubmit = false;
    }
  }

  onChangeDate(value: string, key: string) {
    this.guide[key] = value;
  }

  checkValidate() {
    let validated = true;
    if (
      !this.guide.title ||
      !this.guide.piority ||
      (!this.guide.groupId && this.guide.groupId != 0) ||
      !this.guide.entityTypeId ||
      !this.guide.description ||
      !this.guide.shortDescription
    ) {
      validated = false;
    }
    return validated;
  }

  cancelGuide() {
    if (this.isEdit) {
        this.$router.push(`/information/guidelines/${this.$route.params.guideId}`);
      } else {
        this.$router.push(`/information/guidelines`);
      }
  }

  async deleteEvent() {
    await this.$store.dispatch({
        type: "guide/deleteGuide",
        idDelete: this.$route.params.guideId
      })
    await this.$router.push(`/information/guidelines`)
  }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
</style>


