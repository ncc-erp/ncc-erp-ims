<template>
  <div>
    <section class="inValid" v-if="!isValid">
      <span
        >Để thực hiện chức năng, máy của bạn cần có Camera và trình duyệt Allow.
        <Icon type="ios-cash" size="25" />
        để checkin ! Lưu ý: Kiểm tra link phải là https thì mới nhận
        camera</span
      >
    </section>
    <section :style="styling" v-if="isValid" class="photo-capture">
      <video
        v-show="showVideo"
        ref="player"
        class="camera"
        autoplay
        playsinline
      />
      <canvas v-show="!showVideo" class="preview" ref="canvas" />
      <div v-if="!hideBtns" class="center photo-capture-actions">
        <button
          class="btn flex-center button1"
          @click.prevent="capture"
          v-if="showVideo"
        >
          {{ captureBtnContent }}
        </button>
        <div class="controls" v-else>
          <!-- <button class="btn button1" @click.prevent="cancel">{{cancelBtnContent}}</button>
        <button class="btn button1" @click.prevent="done">{{doneBtnContent}}</button> -->
        </div>
      </div>
    </section>
    <!-------->
    <Modal v-model="isShowConfirmAsk" :title="$t('common.notification')">
      <p>{{ $t("common.verify") }}</p>
      <p>{{ name }} ?</p>
      <Row slot="footer" class="button-zone">
        <Button
          class="button btn-add"
          size="default"
          @click="appRovedorReject(true, data.imageVerifyId)"
          >Accept</Button
        >
        <Button
          type="error"
          size="default"
          ghost
          @click="appRovedorReject(false, data.imageVerifyId)"
          >Reject</Button
        >
      </Row>
    </Modal>
    <!---popup xác nhận thành công----->
    <Modal v-model="isShowConfirmSuccess" :title="$t('common.notification')">
      <p>{{ $t("common.sucess") }}</p>
      <p>Xin chào {{ name }}</p>
      <Row slot="footer" class="button-zone">
        <Button
          type="error"
          size="default"
          ghost
          @click="isShowConfirmSuccess = false"
          >OK</Button
        >
      </Row>
    </Modal>
  </div>
</template>

<script>
export default {
  name: "checkin",
  props: {
    hideBtns: {
      type: Boolean,
      isRequired: false,
      default: false,
    },
    styling: {
      type: Object,
      isRequired: false,
    },
    value: {
      default: null,
    },
    hideButtons: {
      type: Boolean,
      default: false,
    },
    buttonsClasses: {
      type: String,
      default: "",
    },
    captureBtnContent: {
      default: "CHECK IN",
    },
    cancelBtnContent: {
      default: "Chụp lại",
    },
    doneBtnContent: {
      default: "OK",
    },
  },
  data() {
    return {
      showVideo: true,
      picture: null,
      isValid: true,
      isShowConfirmSuccess: false,
      isShowConfirmAsk: false,
      data: {},
      name: "",
    };
  },
  mounted() {
    this.videoPlayer = this.$refs.player;
    this.canvasElement = this.$refs.canvas;
    this.streamUserMediaVideo();
  },
  methods: {
    streamUserMediaVideo() {
      if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
        return;
      }
      navigator.mediaDevices
        .getUserMedia({ video: true })
        .then((stream) => (this.videoPlayer.srcObject = stream))
        .catch(() => {
          this.isValid = false;
        });
    },
    capture() {
      this.showVideo = false;
      this.canvasElement.width = this.videoPlayer.videoWidth;
      this.canvasElement.height = this.videoPlayer.videoHeight;
      var context = this.canvasElement.getContext("2d");
      context.translate(this.canvasElement.width, 0);
      context.scale(-1, 1);

      context.drawImage(this.$refs.player, 0, 0);

      this.stopVideoStream();
      this.picture = this.$refs.canvas.toDataURL("image/jpeg");
      this.done();
    },
    async done() {
      // this.$emit("input", this.picture);
      this.showVideo = true;
      this.streamUserMediaVideo();
      let respone = await this.$store
        .dispatch({
          type: "dashboard/checkIn",
          data: {
            img: this.picture,
          },
        })
        .catch((error) => {
          return;
        });
      this.data = respone;
      if (respone) {
        this.name = respone.lastName + respone.firstName;
        if (respone.showMessage == true) {
          this.isShowConfirmAsk = true;
        } else {
          this.isShowConfirmSuccess = true;
        }
      }
    },
    async appRovedorReject(isApprove, imageId) {
      let data = {
        imageId: imageId,
        isApprove: isApprove,
      };
      await this.$store.dispatch({
        type: "dashboard/decideImage",
        data: data,
      });
      this.imageBase64 = "";
      this.showVideo = true;
      this.isShowConfirmAsk = false;
    },
    cancel() {
      this.showVideo = true;
      this.streamUserMediaVideo();
    },
    stopVideoStream() {
      if (!(this.$refs.player && this.$refs.player.srcObject)) return;
      this.$refs.player.srcObject.getVideoTracks().forEach((track) => {
        track.stop();
      });
    },
  },
};
</script>
<style>
.inValid {
  text-align: center;
  color: indianred;
  font-size: 20px;
  margin-top: 40px;
}
.photo-capture {
  text-align: center;
  max-height: 350px;
}
.camera {
  max-height: 350px !important;
}
video {
  max-height: 350px !important;
  -webkit-transform: scaleX(-1);
  transform: scaleX(-1);
}
.button1 {
  background-color: indianred !important;
  color: aliceblue;
  margin-left: 5px;
}
.preview {
  max-height: 400px;
  max-width: 500px;
}
p {
  font-size: 20px;
  margin-top: 10px;
}
</style>
