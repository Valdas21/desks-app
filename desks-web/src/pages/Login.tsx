import { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>();
  const navigate = useNavigate();

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(undefined);

    try {
      const res = await axios.post("/api/Users/login", { email, password });
      const userId = res.data.userId;
      localStorage.setItem("userId", userId);
      localStorage.setItem("email", email);
      localStorage.setItem("firstName", res.data.firstName);
      localStorage.setItem("lastName", res.data.lastName);
      navigate("/desks");
    } catch (err: any) {
      const msg = err?.response?.data ?? err?.message ?? "Login failed";
      setError(String(msg));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container mt-5" style={{ maxWidth: 420 }}>
      <div className="card">
        <div className="card-body">
          <h4 className="card-title mb-3">Login</h4>
          {error && <div className="alert alert-danger">{error}</div>}
          <form onSubmit={onSubmit}>
            <div className="mb-3">
              <label className="form-label">Email</label>
              <input
                type="email"
                className="form-control"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
                autoFocus
              />
            </div>
            <div className="mb-3">
              <label className="form-label">Password</label>
              <input
                type="password"
                className="form-control"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
            </div>
            <button
              className="btn btn-primary w-100"
              disabled={loading}
              type="submit"
            >
              {loading ? "Signing in..." : "Login"}
            </button>
          </form>
          <div className="mt-3 text-center">
            <a href="/">Need an account? Register</a>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Login;
