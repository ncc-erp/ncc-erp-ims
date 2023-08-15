<template>
  <div class="edit-policies" v-loading="isLoading" v-if="!isLoading">
    <h2>{{ $t("email.emailSetting") }}</h2>
    <Divider />
    <Row>
      <Col class="col" span="12">
        <Col span="24" class="col col__item title-left-dd col-label text-right">
          <Button class="btn btn--create" @click="changeEmailSetting()"
            ><p>
              {{ $t("common.save") }} {{ $t("email.emailSetting") }}
            </p></Button
          >
        </Col>
        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col
              span="6"
              class="col col__item title-left-dd col-label text-right"
            >
              <p>{{ $t("email.host") }}</p>
            </Col>
            <Col span="18" class="col col__item">
              <Input v-model="fromData.host" class="input-data" />
              <p v-if="!fromData.host && !checkValidate" class="error-text">
                {{ $t("common.errorEmpty", { field: $t("email.host") }) }}
              </p>
            </Col>
          </Row>
        </Col>
        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col
              span="6"
              class="col col__item title-left-dd col-label text-right"
            >
              <p>{{ $t("email.port") }}</p>
            </Col>
            <Col span="18" class="col col__item">
              <Input v-model="fromData.port" class="input-data" />
              <p v-if="!fromData.port && !checkValidate" class="error-text">
                {{ $t("common.errorEmpty", { field: $t("email.port") }) }}
              </p>
            </Col>
          </Row>
        </Col>
        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col
              span="6"
              class="col col__item title-left-dd col-label text-right"
            >
              <p>{{ $t("email.fromDisplayName") }}</p>
            </Col>
            <Col span="18" class="col col__item">
              <Input v-model="fromData.fromDisplayName" class="input-data" />
              <p
                v-if="!fromData.fromDisplayName && !checkValidate"
                class="error-text"
              >
                {{
                  $t("common.errorEmpty", {
                    field: $t("email.fromDisplayName"),
                  })
                }}
              </p>
            </Col>
          </Row>
        </Col>
        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col
              span="6"
              class="col col__item title-left-dd col-label text-right"
            >
              <p>{{ $t("email.userName") }}</p>
            </Col>
            <Col span="18" class="col col__item">
              <Input v-model="fromData.userName" class="input-data" />
              <p v-if="!fromData.userName && !checkValidate" class="error-text">
                {{ $t("common.errorEmpty", { field: $t("email.userName") }) }}
              </p>
            </Col>
          </Row>
        </Col>
        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col
              span="6"
              class="col col__item title-left-dd col-label text-right"
            >
              <p>{{ $t("email.password") }}</p>
            </Col>
            <Col span="18" class="col col__item">
              <Input
                v-model="fromData.password"
                type="password"
                password
                class="input-data"
              />
              <p v-if="!fromData.password && !checkValidate" class="error-text">
                {{ $t("common.errorEmpty", { field: $t("email.password") }) }}
              </p>
            </Col>
          </Row>
        </Col>
        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col
              span="6"
              class="col col__item title-left-dd col-label text-right"
            >
              <p>{{ $t("email.defaultFromAddress") }}</p>
            </Col>
            <Col span="18" class="col col__item">
              <Input v-model="fromData.defaultFromAddress" class="input-data" />
              <p
                v-if="!fromData.defaultFromAddress && !checkValidate"
                class="error-text"
              >
                {{
                  $t("common.errorEmpty", {
                    field: $t("email.defaultFromAddress"),
                  })
                }}
              </p>
            </Col>
          </Row>
        </Col>
        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col
              span="6"
              class="col col__item title-left-dd col-label text-right"
            >
              <p>{{ $t("email.enableSsl") }}</p>
            </Col>
            <Col span="18" class="col col__item">
              <Select v-model="fromData.enableSsl" class="input-data">
                <Option
                  v-for="item in enableList"
                  :value="item.value"
                  :key="item.value"
                  >{{ item.label }}</Option
                >
              </Select>
              <p
                v-if="!fromData.enableSsl && !checkValidate"
                class="error-text"
              >
                {{ $t("common.errorEmpty", { field: $t("email.enableSsl") }) }}
              </p>
            </Col>
          </Row>
        </Col>
      </Col>
      <Col class="col" span="12">
        <Col span="24" class="col col__item title-left-dd col-label text-right">
          <Button class="btn btn--create" @click="changeSettingKomu()"
            ><p>{{ $t("common.save") }} {{ $t("email.komu") }}</p></Button
          >
        </Col>
        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col
              span="6"
              class="col col__item title-left-dd col-label text-right"
            >
              <p>{{ $t("email.enableToNoticeKomu") }}</p>
            </Col>
            <Col span="18" class="col col__item">
              <Select v-model="komuData.enableToNoticeKomu" class="input-data">
                <Option
                  v-for="item in enableList"
                  :value="item.value"
                  :key="item.value"
                  >{{ item.label }}</Option
                >
              </Select>
            </Col>
          </Row>
        </Col>
        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col
              span="6"
              class="col col__item title-left-dd col-label text-right"
            >
              <p>{{ $t("email.enableAllowCheckInIMSForAll") }}</p>
            </Col>
            <Col span="18" class="col col__item">
              <Select
                v-model="komuData.enableAllowCheckInIMSForAll"
                class="input-data"
              >
                <Option
                  v-for="item in enableList"
                  :value="item.value"
                  :key="item.value"
                  >{{ item.label }}</Option
                >
              </Select>
            </Col>
          </Row>
        </Col>

        <!-- <Col class="col" span="24">
                    <Row class="row mb-2">
                    <Col span="6" class="col col__item title-left-dd col-label text-right">
                        <p>{{$t("email.userBot")}}</p>
                    </Col>
                    <Col span="18" class="col col__item">
                        <Input v-model="komuData.userBot" class="input-data" />
                        <p v-if="!komuData.userBot && !checkValidateKomu"
                            class="error-text"
                        >{{$t("common.errorEmpty", { field: $t("email.userBot")})}}</p>
                    </Col>
                    </Row>
                </Col>
                <Col class="col" span="24">
                    <Row class="row mb-2">
                    <Col span="6" class="col col__item title-left-dd col-label text-right">
                        <p>{{$t("email.passwordBot")}}</p>
                    </Col>
                    <Col span="18" class="col col__item">
                        <Input v-model="komuData.passwordBot" class="input-data" />
                        <p v-if="!komuData.passwordBot && !checkValidateKomu"
                            class="error-text"
                        >{{$t("common.errorEmpty", { field: $t("email.passwordBot")})}}</p>
                    </Col>
                    </Row>
                </Col>
                <Col class="col" span="24">
                    <Row class="row mb-2">
                    <Col span="6" class="col col__item title-left-dd col-label text-right">
                        <p>{{$t("email.room")}}</p>
                    </Col>
                    <Col span="18" class="col col__item">
                        <Input v-model="komuData.room" class="input-data" />
                        <p>{{$t("email.roomNote")}} </p>
                        <p v-if="!komuData.room && !checkValidateKomu"
                            class="error-text"
                        >{{$t("common.errorEmpty", { field: $t("email.room")})}}</p>
                    </Col>
                    </Row>
                </Col> -->
      </Col>
    </Row>
    <Row>
      <Col class="col" span="12">
        <Col span="24" class="col col__item title-left-dd col-label text-right">
          <Button class="btn btn--create" @click="changeClientId()"
            ><p>Lưu ClientId</p></Button
          >
        </Col>
        <Col class="col" span="24">
          <Row class="row mb-2">
            <Col
              span="6"
              class="col col__item title-left-dd col-label text-right"
            >
              <p>ClientId</p>
            </Col>
            <Col span="18" class="col col__item">
              <Input v-model="clientIdData.clientId" class="input-data" />
              <p
                v-if="!clientIdData.clientId && !checkValidateClientId"
                class="error-text"
              >
                {{
                  $t("common.errorEmpty", {
                    field: $t("ClientId"),
                  })
                }}
              </p>
            </Col>
          </Row>
        </Col>
      </Col>
    </Row>
  </div>
</template>
<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import EventModule from "@/store/modules/event";
@Component({
  components: {},
  props: {},
})
export default class Email extends Vue {
  private isLoading: boolean = false;
  fromData = {
    fromDisplayName: "",
    userName: "",
    port: ".",
    host: "",
    password: "",
    enableSsl: "",
    defaultFromAddress: "",
  };
  enableList = [
    {
      value: "true",
      label: "true",
    },

    {
      value: "false",
      label: "false",
    },
  ];
  komuData = {
    // userBot: "",
    // passwordBot: "",
    // room: "",
    enableToNoticeKomu: "",
  };

  public clientIdData = {
      clientId: ""
  };

  created() {
    this.getEmailSetting();
    this.getSettingKomu();
    this.getClientId();
  }

  async getEmailSetting() {
    this.isLoading = true;
    const response = await this.$store.dispatch({
      type: "email/getEmailSetting",
    });
    this.fromData = response;
    this.isLoading = false;
  }

  async getSettingKomu() {
    this.isLoading = true;
    const response = await this.$store.dispatch({
      type: "email/getSettingKomu",
    });
    this.komuData = response;
    this.isLoading = false;
  }

  async getClientId() {
    this.isLoading = true;
    const response = await this.$store.dispatch({
      type: "email/getClientId",
    });
    this.clientIdData = response;
    this.isLoading = false;
    
  }

  async changeEmailSetting() {
    if (this.checkValidate) {
      this.isLoading = true;
      await this.$store.dispatch({
        type: "email/changeEmailSetting",
        data: this.fromData,
      });

      this.getEmailSetting();
    }
  }

  async changeSettingKomu() {
    // if (this.checkValidateKomu) {
    this.isLoading = true;

    await this.$store.dispatch({
      type: "email/changeSettingKomu",
      data: this.komuData,
    });

    this.getSettingKomu();
    // }
  }

  async changeClientId() {
    if (this.checkValidateClientId) {
      this.isLoading = true;
      await this.$store.dispatch({
        type: "email/changeClientId",
        data: this.clientIdData,
      });

      this.getClientId();
    }
  }

  get checkValidate() {
    let validated = true;
    if (
      !this.fromData.fromDisplayName ||
      !this.fromData.userName ||
      !this.fromData.port ||
      !this.fromData.host ||
      !this.fromData.password ||
      !this.fromData.enableSsl ||
      !this.fromData.defaultFromAddress
      // ||
      // !this.komuData.userBot ||
      // !this.komuData.passwordBot ||
      // !this.komuData.room
    ) {
      validated = false;
    }
    return validated;
  }

  get checkValidateClientId() {
    let validated = true;
    if (!this.clientIdData.clientId) {
      validated = false;
    }
    return validated;
  }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
.input-data {
  width: 400px;
}
.btn--create {
  margin: 0px 20px 30px 0px;
}
</style>