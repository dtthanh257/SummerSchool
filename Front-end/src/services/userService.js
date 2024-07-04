import { BASE_URL } from "./base.js";
import axios from "axios";

class UserService {
  // Đăng nhập tài khoản
  async loginSubmit(data) {
    return await axios({
      method: "post",
      url: `${BASE_URL}/Login`,
      headers: {
        accepts: "*/*",
        "Content-Type": "application/json",
      },
      data: data,
    });
  }
}
export default new UserService();
