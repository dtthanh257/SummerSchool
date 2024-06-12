<template>
    <div>
        <Navbar />
        <div class="introduction">
            <h1>Giới thiệu về trường</h1>
            <div class="content">
                <p>Chào mừng bạn đến với trường của chúng tôi, nơi cung cấp các khóa học tiên tiến về Trí tuệ nhân tạo
                    (AI). Trường chúng tôi tự hào là một trong những cơ sở hàng đầu trong lĩnh vực nghiên cứu và giảng
                    dạy AI, cung cấp môi trường học tập năng động và sáng tạo.</p>
                <p>Khóa học về AI của chúng tôi bao gồm các chủ đề từ cơ bản đến nâng cao, được thiết kế để giúp sinh
                    viên hiểu rõ về lý thuyết và ứng dụng thực tiễn của AI trong nhiều lĩnh vực khác nhau. Đội ngũ giảng
                    viên của chúng tôi là những chuyên gia hàng đầu trong lĩnh vực này, luôn sẵn sàng hỗ trợ và hướng
                    dẫn sinh viên.</p>
                <p>Chúng tôi hy vọng bạn sẽ tìm thấy niềm đam mê và sự hứng thú trong các khóa học của chúng tôi và trở
                    thành những chuyên gia tương lai trong lĩnh vực Trí tuệ nhân tạo.</p>
            </div>
        </div>
    </div>
</template>

<script>
import Navbar from "../components/Navbar.vue";

const Introduction = {
    name: 'Introduction',
    components: {
        Navbar
    }
}
export default Introduction;
</script>

<style scoped>
.introduction {
    padding: 20px;
    max-width: 800px;
    margin: auto;
}

h1 {
    text-align: center;
    margin-bottom: 20px;
}

.content {
    font-size: 1.2em;
    line-height: 1.6;
}

p {
    margin-bottom: 20px;
}
</style>
