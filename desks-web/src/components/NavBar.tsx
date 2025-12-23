import { useNavigate } from "react-router";

function NavBar() {
    const navigate = useNavigate();
    const Logout = () => {
        localStorage.removeItem("userId");
        window.location.href = "/login";
    }
  return (
    <>
      <nav
        className="navbar bg-dark border-bottom border-body"
        data-bs-theme="dark"
      >
        <div className="container-fluid">
          <a className="navbar-brand" href="#">
            Desks Web
          </a>
          <div className="d-flex">
            <button className="btn btn-success" type="button" onClick={() => navigate("/profile")}>
              Profile
            </button>
            <button className="btn btn-success" type="button" onClick={() => navigate("/desks")}>
              Desks
            </button>
            <button className="btn btn-danger" type="button" onClick={() => Logout()}>
              Logout
            </button>
          </div>
        </div>
      </nav>
    </>
  );
}
export default NavBar;
