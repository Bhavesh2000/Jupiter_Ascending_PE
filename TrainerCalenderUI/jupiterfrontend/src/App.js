import React, { useState } from "react";
import { Routes, Route } from "react-router-dom";

import Login from "./components/Login";
import Dashboard from "./components/Dashboard";
import Home from "./components/Home";
import About from "./components/About";
import Header from "./components/Header";
import Footer from "./components/Footer";
import AddTrainerComp from "./components/AddTrainerComp"

function App() {
  const [isLoggedIn,setIsLoggedIn] = useState(false)

  const handleCallBack = (value)=>{
     setIsLoggedIn(value)
  }

  return (
    <div className="App">
      <div>
        <Header isLoggedIn={isLoggedIn}/>
        <div className="content">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/Login" element={<Login callBack={handleCallBack}/>} />
            <Route path="/dashboard" element={<Dashboard />} />
            <Route path="/about" element={<About />} />
            <Route path="/addTrainer" element={<AddTrainerComp/>}/>
          </Routes>
        </div>
        <Footer/>
      </div>
    </div>
  );
}
/* Since react-router-dom is updated to v6 Routes are used instead of Switches and elements are used intead of components */
export default App;
