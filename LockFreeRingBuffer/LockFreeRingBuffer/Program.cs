await using var logger = new AsyncLogger();
logger.Log("Info: Application started");
logger.Log("Warning: Low memory");
logger.Log("Error: Out of memory");
logger.Log("Debug: Memory usage: 1.5 GB");
logger.Log("Info: Application stopped");