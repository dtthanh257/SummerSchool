<template>
    <div class="login-page flex-r">
        <div class="login-container flex-c">
            <h2>Đăng nhập</h2>
            <input v-model="LoginData.username" type="text" placeholder="Nhập tên đăng nhập">
            <input v-model="LoginData.password" type="password" placeholder="Nhập mật khẩu">
            <div class="flex-r jc-sb" style="width: 100%;">
                <a href="">Quên mật khẩu</a>
                <a href="">Đăng nhập với SMS</a>
            </div>
            <button @click="submitLogin" class="login-btn">Đăng nhập</button>
            <div class="flex-r gap-8">
                <div class="a">Bạn chưa có tài khoản?</div>
                <a href=""> Đăng ký</a>
            </div>
        </div>
    </div>
</template>

<script>

import UserService from "@/services/userService.js"

export default {
    name: 'LoginView',
    components: {


    },
    data() {
        return {
            LoginData: {
                username: "",
                password: "",
            },
        };
    },
    methods: {
        async submitLogin() {
            const loginDataSubmit = {
                nickname: this.LoginData.username,
                password: this.LoginData.password,
            };

            try {
                const res = await UserService.loginSubmit(loginDataSubmit);
                console.log("Đăng nhập thành công");
                console.log(res.data);
                localStorage.setItem("id", res.data.id);
                localStorage.setItem("nickname", res.data.nickname);
                localStorage.setItem("jwt", res.data.jwt);
                this.$router.push('/'); // Chuyển hướng đến trang home
            } catch (error) {
                console.error("Đăng nhập thất bại:", error);
            }
        },
    },
}
</script>