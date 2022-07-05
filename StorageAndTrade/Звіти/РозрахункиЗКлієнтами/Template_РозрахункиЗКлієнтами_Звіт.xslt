﻿<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="html" indent="yes" doctype-system="html" />

	<xsl:template name="Head">
		<meta charset="utf-8" />
		<meta name="viewport" content="width=device-width, initial-scale=1" />
		<link rel="stylesheet" href="template/style/bootstrap.min.css" />
		<script src="template/style/jquery.slim.min.js"></script>
		<script src="template/style/popper.min.js"></script>
		<script src="template/style/bootstrap.bundle.min.js"></script>
	</xsl:template>

    <xsl:template match="/root">

		<html>
			<head>
				<title>Table</title>
				<xsl:call-template name="Head" />
			</head>
			<body>

				<div class="container">
					<h2>Звіт</h2>
					<p>Розрахунки з клієнтами</p>

					<table class="table table-bordered table-sm">
						<tr>
							<th>Контрагент</th>
							<th>Валюта</th>
							<th>Сума</th>
						</tr>

						<xsl:for-each select="РозрахункиЗКлієнтами/row">
							<tr>
								<td>
									<xsl:value-of select="Контрагент_Назва"/>
								</td>
								<td>
									<xsl:value-of select="Валюта_Назва"/>
								</td>
								<td align="right">
									<xsl:value-of select="Сума"/>
								</td>
							</tr>
						</xsl:for-each>

					</table>

					<br/>
					<br/>
					<br/>
				    <br/>
				
				</div>

			</body>
		</html>
				
    </xsl:template>
	
</xsl:stylesheet>
