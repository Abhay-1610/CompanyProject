import "./App.css";
import Navigation from "./components/Navigation";
import NavBar from "./components/NavBar";
import { BrowserRouter } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import { useSelector } from "react-redux";
import { useEffect } from "react";
import { startSignalR, stopSignalR } from "./services/signalRService";
import { ToastContainer } from "react-toastify";

function App() {
  const accessToken = useSelector(state => state.auth.accessToken);
   const role = useSelector(state => state.auth.role);

  useEffect(() => {
    if (accessToken && role  === "CompanyUser" || "CompanyAdmin") {
      startSignalR();
    } else {
      stopSignalR();
    }
  }, [accessToken]);

  return (
    <BrowserRouter>
      <ToastContainer position="top-right" autoClose={3000} />
      <NavBar />
      <Navigation />
    </BrowserRouter>
  );
}

export default App;
