<template>
  <div class="edit-policies" :loading="loading">
    <h2
      class="policy-page-title"
    >{{isCreate ? $t("policy.createPageTitle") : $t("policy.editPageTitle")}}</h2>
    <Divider />
    <div class="policy-form">
      <Row v-if="$route.params.policyId" class="form-row">
        <Col span="2" offset="22">
          <Button
            type="error"
            @click="isShowDelete = true"
            style="float: right;"
          >{{ $t("common.delete")}}</Button>
        </Col>
      </Row>

      <Row>
        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("policy.title")}}
            </Col>
            <Col span="20" class="col col__item">
              <Input v-model="Policy.title" />
              <span
                v-if="!Policy.title && isClickSubmit"
                class="error-text"
              >{{$t("common.errorEmpty", { field: $t("policy.title")})}}</span>
            </Col>
          </Row>
        </Col>

        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("policy.avatar")}}
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
            {{ $t("policy.shortDescription")}}
            </Col>
            <Col span="20" class="col col__item">
              <Input
                v-model="Policy.shortDescription"
                :maxlength="500"
                type="textarea"
                :autosize="{minRows: 2,maxRows: 3}"
              />
            </Col>
          </Row>
        </Col>

        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("policy.description")}}
            </Col>
            <Col span="20" class="col col__item">
              <Editor :value="Policy.description" @onChange="handleDescriptionChange" />
              <span
                v-if="!Policy.description && isClickSubmit"
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
              <Select v-model="Policy.entityTypeId" style="width:200px">
                <Option
                  v-for="item in this.types"
                  :value="item.id"
                  :key="item.id"
                >{{ item.typeName }}</Option>
              </Select>
              <span
                v-if="!Policy.entityTypeId && isClickSubmit"
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
              <Select v-model="Policy.groupId" style="width:200px">
                <Option
                  v-for="item in this.groups"
                  :value="item.value"
                  :key="item.value"
                >{{ item.label }}</Option>
              </Select>
              <span
                v-if="Policy.groupId != 0 && !Policy.groupId && isClickSubmit"
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
              <Select v-model="Policy.piority" style="width:200px">
                <Option
                  v-for="item in this.priorities"
                  :value="item.value"
                  :key="item.value"
                >{{ item.label }}</Option>
              </Select>
              <span
                v-if="!Policy.piority && isClickSubmit"
                class="error-text"
              >{{$t("common.errorEmpty", { field: $t("policy.priority")})}}</span>
            </Col>
          </Row>
        </Col>

        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("policy.EffectiveStartDate")}}
            </Col>
            <Col span="8" class="col col__item">
              <DatePicker
                :value="Policy.effectiveStartTime"
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
            {{ $t("policy.EffectiveEndDate")}}
            </Col>
            <Col span="8" class="col col__item">
              <DatePicker
                :value="Policy.effectiveEndTime"
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

         <Col span="24">
          <div class="text-center mt-4">
            <Button
              class="btn btn--create"
              type="primary"
              @click="handleSubmit"
            >{{isCreate ? $t("policy.create") : $t("policy.save")}}</Button>
            <Button class="btn btn--back ml-2" @click="backToList">{{$t("common.cancel")}}</Button>
          </div>
        </Col>
      </Row>
    </div>
    <Modal v-model="isShowDelete" width="360">
      <p>{{$t('policy.deleteConfirmMessage')}}</p>
      <div slot="footer" class="button-center">
        <Button class="btn btn--delete" type="error" @click="handleDelete">{{$t('common.delete')}}</Button>
        <Button class="btn btn--back ml-2" type="default" @click="isShowDelete = false">{{$t('common.cancel')}}</Button>
      </div>
    </Modal>
  </div>
</template>
<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import editorComponent from '../../../tinymce/editorComponent.vue';
import IPolicy from '../../../../store/entities/policy';
import url from '../../../../lib/url';
import VueCroppie from 'vue-croppie'
import 'croppie/croppie.css'

Vue.config.productionTip = false
Vue.use(VueCroppie)

@Component({
  components: {
    'Editor': editorComponent
  }
})
export default class editPolicies extends Vue {
  private editIMG: string = ''
  private Policy: any = {
    effectiveStartTime: '',
    effectiveEndTime: '',
    title: '',
    description: '',
    groupId: 0,
    piority: 1,
    entityTypeId: 1,
    shortDescription: ''
  };
  private types: any = [];
  private groups: any = [
    {
      value: 0,
      label: 'Tất cả'
    }
  ];
  private priorities: any = [
    {
      value: 1,
      label: 'Quan trọng'
    },
    {
      value: 2,
      label: 'Cao'
    },
    {
      value: 3,
      label: 'Thấp'
    }
  ];
  private isClickSubmit: boolean = false;
  private loading: boolean = false;
  private isCreate: boolean = true;
  private invalidTimeRange: boolean = false;
  private isShowDelete: boolean = false
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
        this.Policy.coverImage = this.cropped.replace("data:image/jpeg;base64,","");
  }

  result (output) {
    this.Policy.coverImage = output
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
    await this.$store.dispatch({
      type: "policy/getEntityType",
      entity: 'Policy'
    }).then(res => this.types = res.result)
    if (this.$route.params.policyId) {
      this.loading = true
      this.isCreate = false
      await this.$store.dispatch({
        type: 'policy/getPolicyById',
        id: this.$route.params.policyId
      }).then(res => {
        this.Policy = res
        this.editIMG = res.image
        this.urlIMG = url + res.coverImage
        
        this.Policy.coverImage = null
      })
      this.loading = false
    }
  }

  handleDescriptionChange(value: string) {
    let valueRemovedHttps = value.replace('https://', '')
    let finalDescription = valueRemovedHttps.replace('href="', 'href="https://')
    this.Policy.description = finalDescription
  }

  async handleInputFile(event: any) {
    this.Policy.image = event.target.files[0]
    if (this.Policy.image) {
      this.editIMG = ''
    }
  }

  async handleSubmit() {
    this.isClickSubmit = true
    if (this.checkValidate()) {
      let file = new FormData()
      for (let key in this.Policy) {
        if (key === 'coverImage') {
          if (this.Policy[key]) {
            file.append(key, this.Policy[key]);
          }
        } else file.append(key, this.Policy[key]);
      }

      let response = await this.$store.dispatch({
        type: 'policy/createPolicy',
        data: file
      })
      this.$router.push({ name: 'policyDetail', params: { policyId: response.data.result.id, commentId: null } })
      this.isClickSubmit = false
    }
  }

  onChangeDate(value: string, key: string) {
    this.Policy[key] = value
  }

  checkValidate() {
    let validated = true
    this.invalidTimeRange = new Date(this.Policy.effectiveStartTime).getTime() > new Date(this.Policy.effectiveEndTime).getTime() || !this.Policy.effectiveStartTime || !this.Policy.effectiveEndTime

    if (!this.Policy.title || !this.Policy.piority || !this.Policy.groupId && this.Policy.groupId != 0 || !this.Policy.entityTypeId || !this.Policy.description || this.invalidTimeRange) {
      validated = false
    }
    return validated
  }

  async handleDelete() {
    await this.$store.dispatch({
      type: 'policy/deletePolicy',
      policyId: this.$route.params.policyId
    })
    this.$router.push("/information/policies");
  }

  backToList() {
    if (this.isCreate) {
      this.$router.push("/information/policies")
    } else {
      this.$router.push({ name: 'policyDetail', params: { policyId: this.$route.params.policyId } })
    }
  }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
</style>


