import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const Login = (props) => {
  //const [error, setError] = useState(null);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");


  const navigate = useNavigate();

  const usernameChangeHandler = (event) => {
    setUsername(event.target.value);
  };
  const passwordChangeHandler = (event) => {
    setPassword(event.target.value);
  };

  const submitHandler = (e) => {
    //alert(username+" "+password)
    e.preventDefault();
    //navigate("/dashboard");
    //props.callBack(true)
    localStorage.setItem("isLoggedIn", true); 
    axios
      .post("https://localhost:7250/api/Auth/Login", null, {
        params: { Email: username, Password: password, RememberMe: false },
      })
      .then((res) => {
        console.log(res.data.token);
        if (res.data.token) {
          navigate("/dashboard");
          localStorage.setItem("token", res.data.token);
          props.callBack(true)
        }
      })
      .catch((err) => alert(err));
  };

  return (<>

    <form onSubmit={submitHandler}>
      <div>
        Login
        <br />
        <br />
        <div>
          Username
          <br />
          <input
            name="username"
            type="text"
            placeholder="Enter your Username"
            onChange={usernameChangeHandler}
          />
        </div>
        <div>
          Password
          <br />
          <input
            name="password"
            type="password"
            placeholder="Enter your password"
            onChange={passwordChangeHandler}
          />
        </div>
        {}
        <div><button type="submit">Login</button>
        </div>
      </div>
    </form>
    </>
  );
};

export default Login;
