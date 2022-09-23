import React from "react";
import axios from "axios";
import App from "./App.tsx";

class Dashboard extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      coursemodel: [],
    };
  }

  async componentDidMount() {
    const token = localStorage.getItem("token");
    axios
      .get(`https://localhost:7250/api/Course/CourseList`, {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((res) => {
        console.log(res);
        this.setState({ coursemodel: res.data });
      })
      .catch((err) => alert(err));
  }

  //   async () => {
  //     try {
  //       const response = fetch("http://localhost:3000", { mode: "cors" });
  //       const data = response.json();
  //       console.log({ data });
  //     } catch (e) {
  //       console.log(e);
  //     }
  //   };*/
  render() {
    const isLogged = localStorage.getItem("isLogged");
    return (
      <>
        {isLogged && (
          <div>
            <p>Welcome to the Dashboard Page!</p>
            <h2>Course model</h2>
            <table>
              <thead>
                <tr>
                  <th>id</th>
                  <th>course name</th>
                </tr>
              </thead>
              <tbody>
                {this.state.coursemodel.map((course) => {
                  return (
                    <tr key={course.id}>
                      <td>{course.id}</td>
                      <td>{course.name}</td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
            <App />
          </div>
        )}
      </>
    );
  }
}

export default Dashboard;
