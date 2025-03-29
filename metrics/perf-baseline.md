# 📊 Performance Baseline (before caching)

## 🔧 Test Details
- **Tool**: autocannon
- **Target**: GET http://localhost:5219/products
- **Duration**: 30 seconds
- **Connections**: 20

## 📈 Results

| Metric        | Value         |
|---------------|---------------|
| Requests/sec  | ~214.27 RPS   |
| Total Requests | 6,000         |
| Latency Avg   | ~93 ms        |
| Latency Max   | 438 ms        |
| Traffic       | ~173 MB       |
| Errors        | 0             |

---