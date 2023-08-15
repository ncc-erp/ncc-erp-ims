<template>
  <div class="edit-policies" :loading="loading">
    <h2>{{isCreate ? $t("event.createPageTitle") : $t("event.editPageTitle")}}</h2>
    <Divider />
    <Row v-if="(isEdit && event.canDelete) || (SessionStore.state.user.role == 'Admin' && !isCreate)" class="form-row mb-2">
      <Col span="2" offset="22">
        <Button class="btn btn--delete" type="error" @click="deleteEvent" style="float: right;">{{ $t("policy.delete")}}</Button>
      </Col>
    </Row>
      <Row>
        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col span="4" class="col col__item title-left-dd col-label text-right">
              <p>{{$t("policy.title")}}</p>
            </Col>
            <Col span="20" class="col col__item">
              <Input v-model="event.title" />
              <span
                v-if="!event.title && isClickSubmit"
                class="error-text"
              >{{$t("common.errorEmpty", { field: $t("policy.title")})}}</span>
            </Col>
          </Row>
        </Col>
        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col span="4" class="col col-label text-right col__item title-left-dd">
              <p>{{$t("policy.shortDescription")}}</p>
            </Col>
            <Col span="20" class="col col__item">
              <Input v-model="event.shortDescription" 
                :maxlength="500"
                type="textarea"
                :autosize="{minRows: 2,maxRows: 3}"/>
              <span
                v-if="!event.shortDescription && isClickSubmit"
                class="error-text"
              >{{$t("common.errorEmpty", { field: $t("policy.shortDescription")})}}</span>
            </Col>
          </Row>
        </Col>

        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col span="4" class="col col-label text-right col__item title-left-dd">
              <p>{{$t("policy.avatar")}}</p>
            </Col>
            <Col span="20" class="col col__item">
              <input type="file" @change="handleInputFile" accept="image/*" />
              <div v-if="editIMG">{{ editIMG }}</div>
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
              <p>{{$t("policy.description")}}</p>
            </Col>
            <Col span="20" class="col col__item">
              <Editor :value="event.description" @onChange="handleDescriptionChange" />
              <span
                v-if="!event.description && isClickSubmit"
                class="error-text"
              >{{$t("common.errorEmpty", { field: $t("policy.description")})}}</span>
            </Col>
          </Row>
        </Col>

        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col span="4" class="col col-label text-right col__item title-left-dd">
              <p>{{$t("policy.type")}}</p>
            </Col>
            <Col span="8" class="col col__item">
              <Select v-model="event.entityTypeId" style="width:200px">
                <Option
                  v-for="item in this.types"
                  :value="item.id"
                  :key="item.id"
                >{{ item.typeName }}</Option>
              </Select>
              <span
              v-if="!event.entityTypeId && isClickSubmit"
              class="error-text"
            >{{$t("common.errorEmpty", { field: $t("policy.type")})}}</span>
            </Col>
          </Row>
        </Col>

        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col span="4" class="col col-label text-right col__item title-left-dd">
              <p>{{$t("policy.group")}}</p>
            </Col>
            <Col span="8" class="col col__item">
              <Select v-model="event.groupId" style="width:200px">
                <Option
                  v-for="item in this.groups"
                  :value="item.value"
                  :key="item.value"
                >{{ item.label }}</Option>
              </Select>
              <span
              v-if="this.event.groupId !== 0 && !event.groupId  && isClickSubmit"
              class="error-text"
            >{{$t("common.errorEmpty", { field: $t("policy.group")})}}</span>
            </Col>
          </Row>
        </Col>

        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col span="4" class="col col-label text-right col__item title-left-dd">
              <p>{{$t("policy.priority")}}</p>
            </Col>
            <Col span="8" class="col col__item">
              <Select v-model="event.piority" style="width:200px">
                <Option
                  v-for="item in this.priorities"
                  :value="item.value"
                  :key="item.value"
                >{{ item.label }}</Option>
              </Select>
              <span
              v-if="!event.piority && isClickSubmit"
              class="error-text"
            >{{$t("common.errorEmpty", { field: $t("policy.priority")})}}</span>
            </Col>
          </Row>
        </Col>

        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col span="4" class="col col-label text-right col__item title-left-dd">
              <p>{{$t("policy.EffectiveStartDate")}}</p>
            </Col>
            <Col span="8" class="col col__item">
              <DatePicker
                :value="event.effectiveStartTime"
                @on-change="(e) => {onChangeDate(e, 'effectiveStartTime')}"
                type="date"
                placeholder="Select date"
                style="width: 200px"
              ></DatePicker>
            </Col>
          </Row>
        </Col>

        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col span="4" class="col col-label text-right col__item title-left-dd">
              <p>{{$t("policy.EffectiveEndDate")}}</p>
            </Col>
            <Col span="8" class="col col__item">
              <DatePicker
                :value="event.effectiveEndTime"
                @on-change="(e) => {onChangeDate(e, 'effectiveEndTime')}"
                type="date"
                placeholder="Select date"
                style="width: 200px"
              ></DatePicker>
            </Col>
          </Row>
        </Col>

        <Col class="col" span="24">
          <Row class="row mb-2" v-if="invalidTimeRange && isClickSubmit">
            <Col span="4"></Col>
            <Col span="8" class="col">
              <span class="error-text">{{$t("common.invalidTimeRange")}}</span>
            </Col>
          </Row>
        </Col>

        <Col class="col" span="24">
        <div class="text-center mt-4">
          <Button class="btn btn--create" @click="handleSubmit"><p v-if="!isEdit">{{$t("event.createPageTitle")}}</p><p v-else>{{ $t("event.editPageTitle")}}</p></Button>
          <Button class="btn btn--back ml-2" @click="cancelEvent">{{$t("common.cancel")}}</Button>
        </div>
      </Col>
      </Row>
  </div>
</template>
<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import editorComponent from "../../../tinymce/editorComponent.vue";
import Ievent from "../../../../store/entities/policy";
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
export default class editPolicies extends Vue {
  private isEdit: boolean = false
  private editIMG: string = ''
  private event: any = {
    effectiveStartTime: "",
    effectiveEndTime: "",
    title: "",
    description: "",
    groupId: 0,
    piority: 1,
    entityTypeId: null,
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
  private invalidTimeRange: boolean = false;
  private SessionStore = SessionStore;
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
        this.event.coverImage = this.cropped.replace("data:image/jpeg;base64,","");
  }

  result (output) {
      this.event.coverImage = output
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

  async beforeMount() {
    const entity: string = 'event'
      const entityType = await this.$store.dispatch({
          type: "news/getEntityType",
          entity: entity
        })
      this.types = entityType.result
    if (this.$route.params.eventId) {
      this.isEdit = true
      this.loading = true;
      this.isCreate = false;
      let response = await this.$store.dispatch({
        type: "event/getEvent",
        id: this.$route.params.eventId
      });
      
      this.editIMG = response.image
      this.urlIMG = url + response.coverImage
      this.event = response
      this.event.coverImage = null
      this.loading = false;
    }
  }

  handleDescriptionChange(value: string) {
    this.event.description = value;
  }

  async handleInputFile(event: any) {
    this.event.Image = event.target.files[0];
    if (this.event.image) {
      this.editIMG = ''
    }
  }

  async handleSubmit() {
    this.isClickSubmit = true;
    if (this.checkValidate()) {
      let file = new FormData();
      for (let key in this.event) {
        if (key === 'coverImage') {
          if (this.event[key]) {
            file.append(key, this.event[key]);
          }
        } else file.append(key, this.event[key]);
      }

      const response = await this.$store.dispatch({
        type: "event/createEditEvent",
        dataEvent: file
      });
      if (this.isEdit) {
        this.$router.push(`/information/events/${this.$route.params.eventId}`)
      } else {
        this.$router.push(`/information/events/${response.id}`)
      }
      this.isClickSubmit = false;
    }
  }

  onChangeDate(value: string, key: string) {
    this.event[key] = value;
  }

  checkValidate() {
    let validated = true;
    this.invalidTimeRange = new Date(this.event.effectiveStartTime).getTime() > new Date(this.event.effectiveEndTime).getTime() || !this.event.effectiveStartTime || !this.event.effectiveEndTime
    if (
      !this.event.title ||
      !this.event.piority ||
      (!this.event.groupId && this.event.groupId != 0) ||
      !this.event.entityTypeId ||
      !this.event.description ||
      !this.event.shortDescription || this.invalidTimeRange

    ) {
      
      validated = false;
    }
    return validated;
  }

  cancelEvent() {
    if (this.isEdit) {
        this.$router.push(`/information/events/${this.$route.params.eventId}`)
      } else {
        this.$router.push(`/information/events`)
      }
  }

  async deleteEvent() {
    await this.$store.dispatch({
        type: "event/deleteEvent",
        idDelete: this.$route.params.eventId
      });
    await this.$router.push(`/information/events`)
  }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
</style>


