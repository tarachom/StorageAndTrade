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

					<br/>
					<h2>Товари на складах</h2>
					<br/>
					
					<table class="table table-bordered table-sm">
						<tr class="table-success">
							<th>Номенклатура</th>
							<th>Характеристика</th>
							<th>Склад</th>
							<th style="text-align:center">В наявності</th>
							<th style="text-align:center">До відвантаження</th>
						</tr>

						<xsl:for-each select="ТовариНаСкладах/row">
							<tr>
								<td>
									<xsl:value-of select="Номенклатура_Назва"/>
								</td>
								<td>
									<xsl:value-of select="ХарактеристикаНоменклатури_Назва"/>
								</td>
								<td>
									<xsl:value-of select="Склад_Назва"/>
								</td>
								<td align="right">
									<xsl:value-of select="ВНаявності"/>
								</td>
								<td align="right">
									<xsl:value-of select="ДоВідвантаження"/>
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
