import React from "react";
import { Routes, Route, Link } from "react-router-dom";

import Login from "./Login";
import Dashboard from "./Dashboard";
import Home from "./Home";

function App() {
  return (
    <div className="App">
      <div>
        <div className="header">
          <Link exact activeClassName="active" to="/">
            Home
          </Link>
          <Link activeClassName="active" to="/login">
            Login
          </Link>
          <small>(Access without token only)</small>
          <Link activeClassName="active" to="/dashboard">
            Dashboard
          </Link>
          <small>(Access with token only)</small>
        </div>
        <div className="content">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/Login" element={<Login />} />
            <Route path="/dashboard" element={<Dashboard />} />
          </Routes>
        </div>
      </div>
    </div>
  );
}
/* Since react-router-dom is updated to v6 Routes are used instead of Switches and elements are used intead of components */
export default App;
