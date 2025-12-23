import { Route, Routes, Navigate } from "react-router-dom"
import ListGroup from "./pages/ListGroupDesks"
import Register from "./pages/Register"
import Login from "./pages/Login"
import Profile from "./pages/Profile"


function App() {

  return (
    <>
      <Routes>
        <Route path="/" element={<Navigate to="/register" replace />} />
        <Route path="/register" element={<Register />} />
        <Route path="/login" element={<Login />} />
        <Route path="/desks" element={<ListGroup />} />
        <Route path="/profile" element={<Profile />} />
      </Routes>
    </>
  )
}

export default App
