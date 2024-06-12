<template>
    <div class="navbar">
        <div class="grid-12">
            <div class="grid-10">
                <div class="flex-r">
                    <div class="nav-logo bg-img"> </div>
                </div>
                <div class="flex-r center nav-item">
                    Giới thiệu
                </div>
                <div class="flex-r center nav-item">
                    Các khoá học
                </div>
                <div class="flex-r center nav-item">
                    Diễn đàn
                </div>
                <div class="flex-r account gap-8">
                    <div class="account-ava"></div>
                    <div class="account-nickname" v-if="!isLoggedIn">Tài khoản</div>
                    <div style="white-space: nowrap;" class="account-nickname" v-if="isLoggedIn">Xin chào, {{
                        accountName }}</div>
                    <div class="account-dropdown">
                        <i class="fa-solid fa-angle-down"></i>
                    </div>
                    <div class="account-toggle">
                        <div class="register" v-if="!isLoggedIn">Đăng ký</div>
                        <div class="login" v-if="!isLoggedIn" @click="redirectToLogin">Đăng nhập</div>
                        <div class="register" v-if="isLoggedIn">Tài khoản</div>
                        <div class="login" v-if="isLoggedIn" @click="redirectToLogin">Đăng xuất</div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
const Navbar = {
    name: 'Navbar',
    data() {
        return {
            accountName: 'Tài khoản',
            isLoggedIn: false
        }
    },
    methods: {
        // Chuyển hướng đến trang login
        redirectToLogin() {
            this.$router.push('/login');
        },
        checkLogin() {
            const token = localStorage.getItem('jwt');
            if (token) {
                const name = localStorage.getItem('nickname');
                if (name) {
                    this.accountName = name;
                    this.isLoggedIn = true;
                }
            }
        }
    },
    created() {
        this.checkLogin();
    }
}
export default Navbar;
</script>

<style scoped>
/* Add your styles here */
</style>
