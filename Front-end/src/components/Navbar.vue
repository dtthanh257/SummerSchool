<template>
    <div class="navbar">
        <div class="grid-12">
            <div class="grid-10">
                <div class="flex-r">
                    <div class="nav-logo bg-img"> </div>
                </div>
                <div @click="goToIntroduction" class="flex-r center nav-item">
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
import axios from 'axios';

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
        //Kiểm tra trạng thái đăng nhập
        checkLogin() {
            const token = localStorage.getItem('jwt');
            if (token) {
                const userId = localStorage.getItem('id');
                if (userId) {
                    this.fetchUserInfo(userId);
                    this.isLoggedIn = true;
                }
            }
        },
        async fetchUserInfo(userId) {
            try {

                const response = await axios.get(`http://localhost:5109/api/User/${userId}`, {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem('jwt')}`
                    }
                });
                this.accountName = response.data.name;
            } catch (error) {
                console.error("Error fetching user info:", error);
            }
        },
        // Đăng xuất tài khoản
        logout() {
            localStorage.removeItem('jwt');
            localStorage.removeItem('id');
            this.isLoggedIn = false;
            this.accountName = 'Tài khoản';
            this.$router.push('/');
        },
        //Chuyển sang trang giới thiệu
        goToIntroduction() {
            this.$router.push('/introduction');
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
